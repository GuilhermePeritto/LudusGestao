using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IReservaRepository : ITenantRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetReservasByClienteAsync(Guid clienteId);
    }
} 