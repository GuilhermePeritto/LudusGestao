using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Domain.Interfaces.Repositories;
using BCrypt.Net;
using System.Linq;

namespace LudusGestao.Infrastructure.Security
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private static readonly ConcurrentDictionary<string, DateTime> _refreshTokensInvalidados = new();

        public AuthService(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string> GerarTokenAsync(Usuario usuario)
        {
            Console.WriteLine($"[AuthService] Gerando token para usuário: {usuario.Email}, TenantId: {usuario.TenantId}");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada"));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim("TenantId", usuario.TenantId.ToString()),
                    new Claim("tipo", "access")
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                Issuer = "SistemaReservasIssuer",
                Audience = "SistemaReservasAudience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GerarRefreshTokenAsync(Usuario usuario)
        {
            Console.WriteLine($"[AuthService] Gerando refresh token para usuário: {usuario.Email}");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada"));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("TenantId", usuario.TenantId.ToString()),
                    new Claim("tipo", "refresh")
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Refresh token expira em 30 dias
                Issuer = "SistemaReservasIssuer",
                Audience = "SistemaReservasAudience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidarSenhaAsync(Usuario usuario, string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
        }

        public string GerarHashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool ValidarRefreshToken(string refreshToken)
        {
            try
            {
                Console.WriteLine($"[AuthService] Validando refresh token: {refreshToken.Substring(0, Math.Min(20, refreshToken.Length))}...");
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada"));
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "SistemaReservasIssuer",
                    ValidateAudience = true,
                    ValidAudience = "SistemaReservasAudience",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out var validatedToken);
                
                // Verificar se é um refresh token
                var tipoClaim = principal.FindFirst("tipo")?.Value;
                if (tipoClaim != "refresh")
                {
                    Console.WriteLine($"[AuthService] Token não é do tipo refresh: {tipoClaim}");
                    return false;
                }

                Console.WriteLine($"[AuthService] Refresh token válido para usuário: {principal.FindFirst(ClaimTypes.Email)?.Value}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] Erro ao validar refresh token: {ex.Message}");
                return false;
            }
        }

        public async Task<Usuario?> ObterUsuarioDoRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada"));
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "SistemaReservasIssuer",
                    ValidateAudience = true,
                    ValidAudience = "SistemaReservasAudience",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out var validatedToken);
                
                // Verificar se é um refresh token
                var tipoClaim = principal.FindFirst("tipo")?.Value;
                if (tipoClaim != "refresh")
                {
                    return null;
                }

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return null;
                }

                // Buscar usuário globalmente (sem filtro de tenant)
                return await _usuarioRepository.ObterPorEmailGlobal(principal.FindFirst(ClaimTypes.Email)?.Value ?? "");
            }
            catch
            {
                return null;
            }
        }

        public async Task InvalidarRefreshTokenAsync(string refreshToken)
        {
            // Adicionar o refresh token à blacklist com timestamp
            _refreshTokensInvalidados.TryAdd(refreshToken, DateTime.UtcNow);
            
            // Limpar tokens antigos (mais de 30 dias) para não consumir muita memória
            var tokensParaRemover = _refreshTokensInvalidados
                .Where(kvp => kvp.Value < DateTime.UtcNow.AddDays(-30))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var token in tokensParaRemover)
            {
                _refreshTokensInvalidados.TryRemove(token, out _);
            }
        }

        public async Task<bool> RefreshTokenFoiUsadoAsync(string refreshToken)
        {
            return _refreshTokensInvalidados.ContainsKey(refreshToken);
        }
    }
} 