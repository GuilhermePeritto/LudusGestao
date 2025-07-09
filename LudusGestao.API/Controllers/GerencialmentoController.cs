using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Infrastructure.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using LudusGestao.Domain.Enums;
using LudusGestao.Application.Common.Models;

namespace LudusGestao.API.Controllers;
[ApiController]
[Route("api/gerencialmento")]
[Authorize]
public class GerencialmentoController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public GerencialmentoController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("novo-cliente")]
    public async Task<IActionResult> CriarNovoCliente([FromBody] CreateEmpresaDTO empresaDto)
    {
        try
        {
            // Só permite TenantId = 1
            var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (tenantIdClaim == null || tenantIdClaim != "1")
                return StatusCode(403, new ApiResponse<object>(default) { Success = false, Message = "Acesso restrito ao tenant master" });

        // Busca o maior TenantId em todas as entidades que possuem TenantId
        var maxTenantId = await _db.Empresas.MaxAsync(e => (int?)e.TenantId) ?? 0;
        
        
        // Se não encontrou nenhum TenantId, começa com 1, senão incrementa
        var novoTenantId = maxTenantId + 1;

        // Cria empresa
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nome = empresaDto.Nome,
            Cnpj = empresaDto.Cnpj,
            Email = new LudusGestao.Domain.ValueObjects.Email(empresaDto.Email),
            Endereco = new LudusGestao.Domain.ValueObjects.Endereco(empresaDto.Endereco, "", "", "", "", ""),
            Situacao = SituacaoBase.Ativo,
            TenantId = novoTenantId
        };
        _db.Empresas.Add(empresa);
        await _db.SaveChangesAsync();

        // Cria filial matriz
        var filial = new Filial
        {
            Id = Guid.NewGuid(),
            Nome = $"Matriz {empresa.Nome}",
            Codigo = "001",
            Endereco = empresaDto.Endereco,
            Cidade = "",
            Estado = "",
            Cep = "",
            Telefone = "",
            Email = $"matriz@{empresa.Nome.Replace(" ", "").ToLower()}.com.br",
            Cnpj = empresaDto.Cnpj,
            Responsavel = "Administrador",
            Situacao = SituacaoBase.Ativo,
            DataAbertura = DateTime.UtcNow,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow,
            TenantId = novoTenantId,
            Empresa = empresa
        };
        _db.Filiais.Add(filial);
        _db.Entry(filial).Property("EmpresaId").CurrentValue = empresa.Id;
        await _db.SaveChangesAsync();

        // Cria usuário admin
        var senha = "Admin@123";
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        var emailUsuario = $"administrador@{empresa.Nome.Replace(" ", "").ToLower()}.com.br";
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Administrador",
            Email = emailUsuario,
            Telefone = "",
            Cargo = "Administrador",
            FilialId = filial.Id,
            GrupoId = Guid.NewGuid(),
            Situacao = SituacaoBase.Ativo,
            UltimoAcesso = DateTime.UtcNow,
            Foto = "",
            PermissoesCustomizadas = new List<int>(),
            SenhaHash = senhaHash,
            TenantId = novoTenantId,
            RefreshToken = null,
            RefreshTokenExpiraEm = null
        };
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        // Monta retorno
        var empresaRetorno = new EmpresaDTO
        {
            Id = empresa.Id,
            Nome = empresa.Nome,
            Cnpj = empresa.Cnpj,
            Email = empresa.Email.Endereco,
            Endereco = empresa.Endereco.Rua,
            Situacao = empresa.Situacao
        };
        var filialRetorno = new FilialDTO
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Telefone = filial.Telefone,
            Email = filial.Email,
            Situacao = filial.Situacao,
            EmpresaId = empresa.Id
        };
        var usuarioRetorno = new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone,
            Cargo = usuario.Cargo,
            FilialId = usuario.FilialId,
            GrupoId = usuario.GrupoId,
            Situacao = usuario.Situacao,
            UltimoAcesso = usuario.UltimoAcesso,
            Foto = usuario.Foto,
            PermissoesCustomizadas = usuario.PermissoesCustomizadas
        };

        var resultado = new
        {
            TenantId = novoTenantId,
            Empresa = empresaRetorno,
            FilialMatriz = filialRetorno,
            UsuarioAdmin = usuarioRetorno,
            SenhaPadrao = senha
        };
        
        return Ok(new ApiResponse<object>(resultado, "Novo cliente criado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro interno do servidor" });
        }
    }

    [HttpPost("alterar-senha")]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaRequest dto)
    {
        try
        {
            // Só permite TenantId = 1
            var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (tenantIdClaim == null || tenantIdClaim != "1")
                return StatusCode(403, new ApiResponse<object>(default) { Success = false, Message = "Acesso restrito ao tenant master" });

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NovaSenha))
                return BadRequest(new ApiResponse<object>(default) { Success = false, Message = "Email e nova senha são obrigatórios" });

            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
                return NotFound(new ApiResponse<object>(default) { Success = false, Message = "Usuário não encontrado" });

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            await _db.SaveChangesAsync();
            return Ok(new ApiResponse<object>(default, "Senha alterada com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro interno do servidor" });
        }
    }
}

public class AlterarSenhaRequest
{
    public string Email { get; set; }
    public string NovaSenha { get; set; }
} 