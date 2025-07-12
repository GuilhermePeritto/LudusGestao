// Arquivo criado para padronização da estrutura de repositórios 
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Infrastructure.Data.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context, ITenantService tenantService) : base(context, tenantService) { }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        var query = _context.Set<Usuario>().AsQueryable();
        query = ApplyTenantFilter(query);
        return await query.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObterPorEmailGlobal(string email)
    {
        return await _context.Set<Usuario>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
} 