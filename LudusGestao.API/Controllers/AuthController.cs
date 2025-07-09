using Microsoft.AspNetCore.Mvc;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Application.DTOs.Auth;
using LudusGestao.Domain.Entities;
using LudusGestao.Application.Common.Models;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/autenticacao")]
public class AutenticacaoController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAuthService _authService;

    public AutenticacaoController(IUsuarioRepository usuarioRepository, IAuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _authService = authService;
    }

    [HttpPost("entrar")]
    public async Task<IActionResult> Entrar([FromBody] LoginDTO dto)
    {
        try
        {
            // Buscar usuário globalmente pelo e-mail
            var usuario = await _usuarioRepository.ObterPorEmailGlobal(dto.Email);
            if (usuario == null)
                return Unauthorized(new ApiResponse<object>(default) { Success = false, Message = "Usuário ou senha inválidos" });

            // Setar o tenant do usuário encontrado
            (_authService as ITenantService)?.SetTenant(usuario.TenantId.ToString());

            var senhaValida = await _authService.ValidarSenhaAsync(usuario, dto.Senha);
            if (!senhaValida)
                return Unauthorized(new ApiResponse<object>(default) { Success = false, Message = "Usuário ou senha inválidos" });

            var token = await _authService.GerarTokenAsync(usuario);
            var tokenDto = new TokenDTO
            {
                AccessToken = token,
                ExpiraEm = DateTime.UtcNow.AddHours(2)
            };
            return Ok(new ApiResponse<TokenDTO>(tokenDto, "Login realizado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro interno do servidor" });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDTO dto)
    {
        try
        {
            // Busca usuário pelo refresh token
            var usuario = (await _usuarioRepository.ListarPorTenant(0)).FirstOrDefault(u => u.RefreshToken == dto.RefreshToken && u.RefreshTokenExpiraEm > DateTime.UtcNow);
            if (usuario == null)
                return Unauthorized(new ApiResponse<object>(default) { Success = false, Message = "Refresh token inválido ou expirado" });

            // Gera novo access token e refresh token
            usuario.RefreshToken = _authService.GerarRefreshToken();
            usuario.RefreshTokenExpiraEm = DateTime.UtcNow.AddDays(7);
            var token = await _authService.GerarTokenAsync(usuario);
            await _usuarioRepository.Atualizar(usuario);

            var tokenDto = new TokenDTO
            {
                AccessToken = token,
                RefreshToken = usuario.RefreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(2)
            };
            return Ok(new ApiResponse<TokenDTO>(tokenDto, "Token renovado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro interno do servidor" });
        }
    }
} 