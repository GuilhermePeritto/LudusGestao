using LudusGestao.Domain.Entities;
using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GerarTokenAsync(Usuario usuario);
        Task<string> GerarRefreshTokenAsync(Usuario usuario);
        Task<bool> ValidarSenhaAsync(Usuario usuario, string senha);
        string GerarHashSenha(string senha);
        bool ValidarRefreshToken(string refreshToken);
        Task<Usuario?> ObterUsuarioDoRefreshTokenAsync(string refreshToken);
        Task InvalidarRefreshTokenAsync(string refreshToken);
        Task<bool> RefreshTokenFoiUsadoAsync(string refreshToken);
    }
} 