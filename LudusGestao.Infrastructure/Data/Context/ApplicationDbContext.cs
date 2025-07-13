using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Interfaces.Services;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using BCrypt.Net;
using System.Threading;
using System.Threading.Tasks;
using LudusGestao.Domain.Common.Exceptions;


namespace LudusGestao.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantService _tenantService;

        // Construtor para runtime (injeção de dependência)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
        }

        // Construtor para tempo de design (migrations, update-database)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // _tenantService permanece nulo
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Recebivel> Recebiveis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<GrupoPermissao> GruposPermissoes { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<GrupoPermissaoFilial> GruposPermissoesFiliais { get; set; }
        public DbSet<UsuarioPermissaoFilial> UsuariosPermissoesFiliais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Filtro global de tenant
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext).GetMethod(nameof(SetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, new object[] { modelBuilder });
                }
            }


            
            // Configuração para List<string> Comodidades
            modelBuilder.Entity<Local>()
                .Property(l => l.Comodidades)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
            
            // Configuração para List<Guid> Permissoes
            modelBuilder.Entity<GrupoPermissaoFilial>()
                .Property(g => g.Permissoes)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<UsuarioPermissaoFilial>()
                .Property(u => u.Permissoes)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
            
            // Configuração para propriedades opcionais
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Foto)
                .IsRequired(false);
            
            modelBuilder.Entity<Cliente>()
                .Property(c => c.Observacoes)
                .IsRequired(false);
            
            modelBuilder.Entity<Cliente>()
                .Property(c => c.DataAtualizacao)
                .IsRequired(false);
            
            modelBuilder.Entity<Reserva>()
                .Property(r => r.Observacoes)
                .IsRequired(false);
            
            modelBuilder.Entity<Local>()
                .Property(l => l.Descricao)
                .IsRequired(false);
            
            modelBuilder.Entity<Local>()
                .Property(l => l.Capacidade)
                .IsRequired(false);
            
            modelBuilder.Entity<Reserva>()
                .Property(r => r.UsuarioId)
                .IsRequired(false);
            
            modelBuilder.Entity<Recebivel>()
                .Property(r => r.ReservaId)
                .IsRequired(false);
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, ITenantEntity
        {
            // Só aplica o filtro se o serviço estiver disponível (runtime)
            if (_tenantService != null)
                modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
        }

        // Removido o método Seed e toda a sua lógica conforme solicitado

        public override int SaveChanges()
        {
            SetAndValidateTenantIds();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAndValidateTenantIds();
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Método específico para seed que não valida tenant
        public async Task<int> SaveChangesWithoutTenantValidationAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Método específico para seed que não chama nenhuma validação
        public async Task<int> SaveChangesForSeedAsync(CancellationToken cancellationToken = default)
        {
            // Salva diretamente sem chamar SetAndValidateTenantIds
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAndValidateTenantIds()
        {
            if (_tenantService == null) return;
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                if (entry.Entity is ITenantEntity tenantEntity)
                {
                    // Se o TenantId já está definido (como no seed), não altera
                    if (tenantEntity.TenantId == 0)
                    {
                        // Tenta atribuir o TenantId do contexto
                        var currentTenantId = _tenantService.GetTenantId();
                        if (currentTenantId == 0)
                            throw new LudusGestao.Domain.Common.Exceptions.ValidationException("TenantId não pode ser 0. Operação não permitida.");
                        tenantEntity.TenantId = currentTenantId;
                    }
                    // Remove a validação que impedia TenantId = 0, pois no seed definimos manualmente
                }
            }
        }


    }
} 