using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudusGestao.Infrastructure.Data.Repositories
{
    public class PermissaoRepository : BaseRepository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(ApplicationDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public async Task<IEnumerable<Permissao>> ObterPorModuloPai(string moduloPai)
        {
            var query = _context.Set<Permissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.Where(p => p.ModuloPai == moduloPai).ToListAsync();
        }

        public async Task<IEnumerable<Permissao>> ObterPorSubmodulo(string submodulo)
        {
            var query = _context.Set<Permissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.Where(p => p.Submodulo == submodulo).ToListAsync();
        }

        public async Task<IEnumerable<string>> ObterModulosPai()
        {
            var query = _context.Set<Permissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.Select(p => p.ModuloPai).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ObterSubmodulos()
        {
            var query = _context.Set<Permissao>().AsQueryable();
            query = ApplyTenantFilter(query);
            return await query.Select(p => p.Submodulo).Distinct().Where(s => !string.IsNullOrEmpty(s)).ToListAsync();
        }
    }
} 