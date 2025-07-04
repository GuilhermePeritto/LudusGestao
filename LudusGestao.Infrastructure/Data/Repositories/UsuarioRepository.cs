// Arquivo criado para padronização da estrutura de repositórios 
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Infrastructure.Data.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Usuario>> ListarPorTenant(int tenantId)
    {
        return await _context.Set<Usuario>().Where(u => u.TenantId == tenantId).ToListAsync();
    }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        return await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObterPorEmailGlobal(string email)
    {
        return await _context.Set<Usuario>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
} 