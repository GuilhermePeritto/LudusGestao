using LudusGestao.Application.DTOs.Auth;
using LudusGestao.Application.Common.Models;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Application.DTOs.Usuario;
using System;
using System.Threading.Tasks;
using AutoMapper;

namespace LudusGestao.Application.Services
{
    public class AuthService
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public AuthService(IAuthService authService, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _authService = authService;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TokenDTO>> LoginAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailGlobal(dto.Email);
            if (usuario == null)
                return new ApiResponse<TokenDTO>(null) { Success = false, Message = "Usuário ou senha inválidos" };

            var senhaValida = await _authService.ValidarSenhaAsync(usuario, dto.Senha);
            if (!senhaValida)
                return new ApiResponse<TokenDTO>(null) { Success = false, Message = "Usuário ou senha inválidos" };

            var accessToken = await _authService.GerarTokenAsync(usuario);
            var refreshToken = await _authService.GerarRefreshTokenAsync(usuario);

            // Mapear usuário para DTO
            var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);

            var tokenDto = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(2),
                Usuario = usuarioDto
            };

            return new ApiResponse<TokenDTO>(tokenDto, "Login realizado com sucesso");
        }

        public async Task<ApiResponse<TokenDTO>> RefreshAsync(RefreshTokenDTO dto)
        {
            Console.WriteLine($"[AuthAppService] Iniciando refresh com token: {dto.RefreshToken.Substring(0, Math.Min(20, dto.RefreshToken.Length))}...");
            
            if (await _authService.RefreshTokenFoiUsadoAsync(dto.RefreshToken))
            {
                Console.WriteLine("[AuthAppService] Refresh token já foi utilizado");
                return new ApiResponse<TokenDTO>(null) { Success = false, Message = "Refresh token já foi utilizado" };
            }

            var tokenValido = _authService.ValidarRefreshToken(dto.RefreshToken);
            Console.WriteLine($"[AuthAppService] Validação do refresh token: {tokenValido}");
            
            if (!tokenValido)
            {
                Console.WriteLine("[AuthAppService] Refresh token inválido ou expirado");
                return new ApiResponse<TokenDTO>(null) { Success = false, Message = "Refresh token inválido ou expirado" };
            }

            var usuario = await _authService.ObterUsuarioDoRefreshTokenAsync(dto.RefreshToken);
            if (usuario == null)
            {
                Console.WriteLine("[AuthAppService] Usuário não encontrado do refresh token");
                return new ApiResponse<TokenDTO>(null) { Success = false, Message = "Refresh token inválido ou expirado" };
            }

            Console.WriteLine($"[AuthAppService] Usuário encontrado: {usuario.Email}, invalidando token antigo");
            await _authService.InvalidarRefreshTokenAsync(dto.RefreshToken);

            var novoAccessToken = await _authService.GerarTokenAsync(usuario);
            var novoRefreshToken = await _authService.GerarRefreshTokenAsync(usuario);

            // Mapear usuário para DTO
            var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);

            var tokenDto = new TokenDTO
            {
                AccessToken = novoAccessToken,
                RefreshToken = novoRefreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(2),
                Usuario = usuarioDto
            };

            Console.WriteLine("[AuthAppService] Token renovado com sucesso");
            return new ApiResponse<TokenDTO>(tokenDto, "Token renovado com sucesso");
        }
    }
} 