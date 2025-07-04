// Arquivo criado para padronização da estrutura de repositórios 
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Infrastructure.Data.Repositories;

public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Empresa>> ListarPorTenant(int tenantId)
    {
        // Implementação fictícia, ajuste conforme necessário
        return await Task.FromResult(new List<Empresa>());
    }
} 