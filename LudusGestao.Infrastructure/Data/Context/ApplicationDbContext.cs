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

            modelBuilder.Entity<Empresa>().OwnsOne(e => e.Email);
            modelBuilder.Entity<Empresa>().OwnsOne(e => e.Endereco);
            
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
            
            // Configuração para List<int> PermissoesCustomizadas
            modelBuilder.Entity<Usuario>()
                .Property(u => u.PermissoesCustomizadas)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<int>>(
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

        public void Seed()
        {
            // Evitar duplicidade - usa IgnoreQueryFilters para ignorar filtros de tenant
            if (!Empresas.IgnoreQueryFilters().Any())
            {
                var empresa = new Empresa
                {
                    Id = Guid.NewGuid(),
                    Nome = "Ludus Sistemas",
                    Cnpj = "12345678000199",
                    Email = new LudusGestao.Domain.ValueObjects.Email("contato@ludussistemas.com.br"),
                    Endereco = new LudusGestao.Domain.ValueObjects.Endereco("Rua da Inovação", "500", "Centro", "TechCity", "SP", "12345-678"),
                    Situacao = LudusGestao.Domain.Enums.SituacaoBase.Ativo,
                    TenantId = 1
                };
                Empresas.Add(empresa);
                base.SaveChanges(); // Usa SaveChanges da base para evitar validação

                var filial = new Filial
                {
                    Id = Guid.NewGuid(),
                    Nome = "Filial Matriz Ludus",
                    Codigo = "001",
                    Endereco = "Rua da Inovação, 500",
                    Cidade = "TechCity",
                    Estado = "SP",
                    Cep = "12345-678",
                    Telefone = "11999998888",
                    Email = "matriz@ludussistemas.com.br",
                    Cnpj = "12345678000199",
                    Responsavel = "Administrador",
                    Situacao = LudusGestao.Domain.Enums.SituacaoBase.Ativo,
                    DataAbertura = DateTime.UtcNow.AddYears(-2),
                    TenantId = 1
                };
                Filiais.Add(filial);
                Entry(filial).Property("EmpresaId").CurrentValue = empresa.Id;
                base.SaveChanges();

                var cliente = new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = "Carlos Silva",
                    Subtitulo = "Premium",
                    Documento = "98765432100",
                    Email = "carlos@cliente.com",
                    Telefone = "11988887777",
                    Endereco = "Rua do Cliente, 123",
                    Observacoes = "Cliente importante da Ludus",
                    Situacao = LudusGestao.Domain.Enums.SituacaoCliente.Ativo,
                    TenantId = 1
                };
                Clientes.Add(cliente);
                base.SaveChanges();

                var local = new Local
                {
                    Id = Guid.NewGuid(),
                    Subtitulo = "Sala de Treinamento",
                    Nome = "Sala Ludus 1",
                    Tipo = "Sala",
                    Intervalo = 60,
                    ValorHora = 200.00m,
                    Capacidade = 30,
                    Descricao = "Sala de treinamento equipada com recursos multimídia.",
                    Comodidades = new List<string> { "Projetor", "Ar-condicionado", "Cadeiras Ergonômicas" }.ToList(),
                    Situacao = LudusGestao.Domain.Enums.SituacaoLocal.Ativo,
                    Cor = "#4CAF50",
                    HoraAbertura = "09:00",
                    HoraFechamento = "18:00",
                    FilialId = filial.Id,
                    TenantId = 1
                };
                Locais.Add(local);
                base.SaveChanges();

                var reserva = new Reserva
                {
                    Id = Guid.NewGuid(),
                    ClienteId = cliente.Id,
                    LocalId = local.Id,
                    TenantId = 1,
                    Data = DateTime.UtcNow.AddDays(2),
                    HoraInicio = "10:00",
                    HoraFim = "12:00",
                    Situacao = LudusGestao.Domain.Enums.SituacaoReserva.Confirmado,
                    Cor = "#4CAF50",
                    Esporte = "Treinamento Corporativo",
                    Observacoes = "Reserva para treinamento de equipe Ludus.",
                    Valor = 400.00m,
                };
                Reservas.Add(reserva);
                base.SaveChanges();

                var recebivel = new Recebivel
                {
                    Id = Guid.NewGuid(),
                    ClienteId = cliente.Id,
                    Descricao = "Pagamento referente à reserva de sala.",
                    Valor = 400.00m,
                    DataVencimento = DateTime.UtcNow.AddDays(10),
                    Situacao = LudusGestao.Domain.Enums.SituacaoRecebivel.Aberto,
                    ReservaId = reserva.Id,
                    TenantId = 1
                };
                Recebiveis.Add(recebivel);
                base.SaveChanges();

                var senha = "Ludus@2024";
                var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
                var usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = "Administrador Ludus",
                    Email = "admin@ludussistemas.com.br",
                    Telefone = "11999990000",
                    Cargo = "Administrador",
                    FilialId = filial.Id,
                    GrupoId = Guid.NewGuid(),
                    Situacao = LudusGestao.Domain.Enums.SituacaoBase.Ativo,
                    UltimoAcesso = DateTime.UtcNow,
                    Foto = null,
                    PermissoesCustomizadas = new List<int> { },
                    SenhaHash = senhaHash,
                    TenantId = 1,
                    RefreshToken = null,
                    RefreshTokenExpiraEm = null
                };
                Usuarios.Add(usuario);
                base.SaveChanges();
            }
        }

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

        private void SetAndValidateTenantIds()
        {
            if (_tenantService == null) return;
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                if (entry.Entity is ITenantEntity tenantEntity)
                {
                    if (tenantEntity.TenantId == 0)
                    {
                        // Tenta atribuir o TenantId do contexto
                        var currentTenantId = _tenantService.GetTenantId();
                        if (currentTenantId == 0)
                            throw new LudusGestao.Domain.Common.Exceptions.ValidationException("TenantId não pode ser 0. Operação não permitida.");
                        tenantEntity.TenantId = currentTenantId;
                    }
                    if (tenantEntity.TenantId == 0)
                        throw new LudusGestao.Domain.Common.Exceptions.ValidationException("TenantId não pode ser 0. Operação não permitida.");
                }
            }
        }
    }
} 