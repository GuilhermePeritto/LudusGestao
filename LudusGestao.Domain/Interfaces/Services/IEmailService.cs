using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string mensagem);
    }
} 