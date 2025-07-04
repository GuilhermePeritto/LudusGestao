using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
} 