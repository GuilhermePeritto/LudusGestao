using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetReservasByClienteAsync(Guid clienteId);
    }
} 