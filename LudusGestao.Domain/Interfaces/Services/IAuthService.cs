using LudusGestao.Domain.Entities;
using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GerarTokenAsync(Usuario usuario);
        Task<bool> ValidarSenhaAsync(Usuario usuario, string senha);
        string GerarHashSenha(string senha);
        string GerarRefreshToken();
    }
} 