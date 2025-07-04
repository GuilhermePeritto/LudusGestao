using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Services;
using BCrypt.Net;

namespace LudusGestao.Infrastructure.Security
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GerarTokenAsync(Usuario usuario)
        {
            Console.WriteLine($"[AuthService] Gerando token para usuário: {usuario.Email}, TenantId: {usuario.TenantId}");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nome", usuario.Nome),
                new Claim("TenantId", usuario.TenantId.ToString())
                // Adicione mais claims conforme necessário
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<bool> ValidarSenhaAsync(Usuario usuario, string senha)
        {
            // Validação segura usando BCrypt
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash));
        }

        public string GerarHashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public string GerarRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
} 