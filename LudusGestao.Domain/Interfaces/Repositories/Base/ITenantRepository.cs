using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Repositories.Base
{
    public interface ITenantRepository<T> : IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListarPorTenant(int TenantId);
    }
} 