using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;

namespace LudusGestao.Infrastructure.Data.Repositories
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApplicationDbContext context, ITenantService tenantService) : base(context, tenantService) { }

        public async Task<IEnumerable<Reserva>> GetReservasByClienteAsync(Guid clienteId)
        {
            var query = _context.Reservas.AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.Where(r => r.ClienteId == clienteId).ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> ListarPorTenant(int tenantId)
        {
            return await _context.Set<Reserva>().Where(r => r.TenantId == tenantId).ToListAsync();
        }
    }
} 