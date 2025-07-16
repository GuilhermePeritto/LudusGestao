using AutoMapper;
using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Application.Common.Models;
using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.DTOs.Gerencialmento;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Enums;
using LudusGestao.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LudusGestao.Application.Services
{
    public class GerencialmentoService : IGerencialmentoService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IFilialRepository _filialRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public GerencialmentoService(
            IEmpresaRepository empresaRepository,
            IFilialRepository filialRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper)
        {
            _empresaRepository = empresaRepository;
            _filialRepository = filialRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<NovoClienteResultadoDTO>> CriarNovoCliente(CriarNovoClienteDTO dto)
        {
            try
            {
                // Busca o maior TenantId em todas as entidades que possuem TenantId
                var maxTenantId = await _empresaRepository.GetMaxTenantIdAsync();
                var novoTenantId = maxTenantId + 1;

                // Cria empresa
                var empresa = new Empresa
                {
                    Id = Guid.NewGuid(),
                    Nome = dto.Nome,
                    Cnpj = dto.Cnpj,
                    Email = dto.Email,
                    Endereco = $"{dto.Rua}, {dto.Numero}",
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Cep = dto.CEP,
                    Telefone = "",
                    Situacao = SituacaoBase.Ativo,
                    TenantId = novoTenantId
                };

                await _empresaRepository.Criar(empresa);

                // Cria filial matriz
                var filial = new Filial
                {
                    Id = Guid.NewGuid(),
                    Nome = $"Matriz {empresa.Nome}",
                    Codigo = "001",
                    Endereco = dto.Rua,
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Cep = dto.CEP,
                    Telefone = "",
                    Email = $"matriz@{empresa.Nome.Replace(" ", "").ToLower()}.com.br",
                    Cnpj = dto.Cnpj,
                    Responsavel = "Administrador",
                    Situacao = SituacaoBase.Ativo,
                    DataAbertura = DateTime.UtcNow,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow,
                    TenantId = novoTenantId,
                    EmpresaId = empresa.Id
                };

                await _filialRepository.Criar(filial);

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
                    EmpresaId = empresa.Id,
                    GrupoPermissaoId = null,
                    Situacao = SituacaoBase.Ativo,
                    UltimoAcesso = DateTime.UtcNow,
                    Foto = "",
                    Senha = senhaHash,
                    TenantId = novoTenantId,
                };

                await _usuarioRepository.Criar(usuario);

                // Monta retorno
                var empresaRetorno = _mapper.Map<EmpresaDTO>(empresa);
                var filialRetorno = _mapper.Map<FilialDTO>(filial);
                var usuarioRetorno = _mapper.Map<UsuarioDTO>(usuario);

                var resultado = new NovoClienteResultadoDTO
                {
                    TenantId = novoTenantId,
                    Empresa = empresaRetorno,
                    FilialMatriz = filialRetorno,
                    UsuarioAdmin = usuarioRetorno,
                    SenhaPadrao = senha
                };

                return new ApiResponse<NovoClienteResultadoDTO>(resultado, "Novo cliente criado com sucesso");
            }
            catch (Exception ex)
            {
                return new ApiResponse<NovoClienteResultadoDTO>(default) 
                { 
                    Success = false, 
                    Message = "Erro interno do servidor" 
                };
            }
        }

        public async Task<ApiResponse<object>> AlterarSenha(AlterarSenhaDTO dto)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
                if (usuario == null)
                {
                    return new ApiResponse<object>(default) 
                    { 
                        Success = false, 
                        Message = "Usuário não encontrado" 
                    };
                }

                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
                await _usuarioRepository.Atualizar(usuario);

                return new ApiResponse<object>(default, "Senha alterada com sucesso");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(default) 
                { 
                    Success = false, 
                    Message = "Erro interno do servidor" 
                };
            }
        }
    }
} 