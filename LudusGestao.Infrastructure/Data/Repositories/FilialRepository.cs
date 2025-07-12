// Arquivo criado para padronização da estrutura de repositórios 
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Infrastructure.Data.Repositories;

public class FilialRepository : BaseRepository<Filial>, IFilialRepository
{
    public FilialRepository(ApplicationDbContext context, ITenantService tenantService) : base(context, tenantService) { }
} 