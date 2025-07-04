// Arquivo criado para padronização da estrutura de repositórios 
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Infrastructure.Data.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Cliente>> ListarPorTenant(int tenantId)
    {
        return await _context.Set<Cliente>().Where(c => c.TenantId == tenantId).ToListAsync();
    }
} 