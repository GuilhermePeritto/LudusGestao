using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IEmpresaRepository : IBaseRepository<Empresa>
    {
        Task<int> GetMaxTenantIdAsync();
    }
} 