using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudusGestao.Infrastructure.Data.Repositories
{
    public class GrupoPermissaoRepository : BaseRepository<GrupoPermissao>, IGrupoPermissaoRepository
    {
        public GrupoPermissaoRepository(ApplicationDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public async Task<IEnumerable<GrupoPermissao>> ObterComUsuarios()
        {
            var query = _context.Set<GrupoPermissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.ToListAsync();
        }

        public async Task<GrupoPermissao?> ObterComUsuariosPorId(Guid id)
        {
            var query = _context.Set<GrupoPermissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
} 