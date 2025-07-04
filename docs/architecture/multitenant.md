# Arquitetura Multitenant - Ludus Gestão

## Visão Geral

O sistema Ludus Gestão implementa uma arquitetura multitenant para permitir que múltiplas empresas utilizem a mesma instância da aplicação, mantendo seus dados isolados e seguros.

## Estrutura Multitenant

### 1. Interface ITenantEntity

Todas as entidades que pertencem a um tenant implementam esta interface:

```csharp
public interface ITenantEntity
{
    int TenantId { get; set; }
}
```

### 2. Entidades com Tenant

```csharp
public class Cliente : BaseEntity, ITenantEntity
{
    // Propriedades da entidade
    public string Nome { get; set; }
    public string Documento { get; set; }
    // ... outras propriedades
    
    // Tenant ID (herdado de ITenantEntity)
    public int TenantId { get; set; }
}
```

### 3. BaseEntity

A classe base que todas as entidades herdam:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
}
```

## Implementação da Segregação

### 1. TenantMiddleware

Middleware responsável por identificar e configurar o tenant atual:

```csharp
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Extrair tenant ID do header, query string ou subdomain
        var tenantId = ExtractTenantId(context);
        
        if (tenantId.HasValue)
        {
            // Configurar tenant no contexto
            tenantService.SetCurrentTenant(tenantId.Value);
        }

        await _next(context);
    }

    private int? ExtractTenantId(HttpContext context)
    {
        // 1. Tentar extrair do header
        if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantHeader))
        {
            if (int.TryParse(tenantHeader, out var tenantId))
                return tenantId;
        }

        // 2. Tentar extrair da query string
        if (context.Request.Query.TryGetValue("tenantId", out var tenantQuery))
        {
            if (int.TryParse(tenantQuery, out var tenantId))
                return tenantId;
        }

        // 3. Tentar extrair do subdomain
        var host = context.Request.Host.Host;
        var subdomain = ExtractSubdomain(host);
        if (!string.IsNullOrEmpty(subdomain))
        {
            // Buscar tenant pelo subdomain
            return GetTenantIdBySubdomain(subdomain);
        }

        return null;
    }
}
```

### 2. TenantService

Serviço responsável por gerenciar o tenant atual:

```csharp
public interface ITenantService
{
    int? GetCurrentTenant();
    void SetCurrentTenant(int tenantId);
    bool IsTenantConfigured();
}

public class TenantService : ITenantService
{
    private readonly AsyncLocal<int?> _currentTenant = new AsyncLocal<int?>();

    public int? GetCurrentTenant()
    {
        return _currentTenant.Value;
    }

    public void SetCurrentTenant(int tenantId)
    {
        _currentTenant.Value = tenantId;
    }

    public bool IsTenantConfigured()
    {
        return _currentTenant.Value.HasValue;
    }
}
```

### 3. BaseRepository com Filtro de Tenant

```csharp
public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, ITenantEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly ITenantService _tenantService;

    public BaseRepository(ApplicationDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var tenantId = _tenantService.GetCurrentTenant();
        if (!tenantId.HasValue)
            throw new InvalidOperationException("Tenant não configurado");

        return await _context.Set<T>()
            .Where(e => e.TenantId == tenantId.Value)
            .ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        if (!tenantId.HasValue)
            throw new InvalidOperationException("Tenant não configurado");

        return await _context.Set<T>()
            .Where(e => e.Id == id && e.TenantId == tenantId.Value)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        if (!tenantId.HasValue)
            throw new InvalidOperationException("Tenant não configurado");

        entity.TenantId = tenantId.Value;
        entity.DataCriacao = DateTime.UtcNow;

        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        if (!tenantId.HasValue)
            throw new InvalidOperationException("Tenant não configurado");

        // Verificar se a entidade pertence ao tenant atual
        var existingEntity = await GetByIdAsync(entity.Id);
        if (existingEntity == null)
            throw new InvalidOperationException("Entidade não encontrada ou não pertence ao tenant atual");

        entity.TenantId = tenantId.Value;
        entity.DataAtualizacao = DateTime.UtcNow;

        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        if (!tenantId.HasValue)
            throw new InvalidOperationException("Tenant não configurado");

        var entity = await GetByIdAsync(id);
        if (entity == null)
            throw new InvalidOperationException("Entidade não encontrada ou não pertence ao tenant atual");

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
```

## Configuração no Startup

### 1. Registro de Serviços

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Registrar serviços de tenant
    services.AddScoped<ITenantService, TenantService>();
    
    // Registrar repositórios com tenant
    services.AddScoped<IClienteRepository, ClienteRepository>();
    services.AddScoped<IReservaRepository, ReservaRepository>();
    // ... outros repositórios
}
```

### 2. Configuração do Middleware

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ... outras configurações
    
    // Adicionar middleware de tenant antes do routing
    app.UseMiddleware<TenantMiddleware>();
    
    app.UseRouting();
    // ... outras configurações
}
```

## Estratégias de Identificação do Tenant

### 1. Header HTTP
```http
GET /api/clientes
X-Tenant-ID: 123
```

### 2. Query String
```http
GET /api/clientes?tenantId=123
```

### 3. Subdomain
```http
empresa1.ludusgestao.com/api/clientes
empresa2.ludusgestao.com/api/clientes
```

### 4. JWT Token
```json
{
  "sub": "user123",
  "tenant_id": 123,
  "exp": 1640995200
}
```

## Segurança e Validação

### 1. Validação de Tenant

```csharp
public class TenantValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var tenantService = context.HttpContext.RequestServices
            .GetRequiredService<ITenantService>();

        if (!tenantService.IsTenantConfigured())
        {
            context.Result = new BadRequestObjectResult(
                new { error = "Tenant não especificado" });
            return;
        }

        base.OnActionExecuting(context);
    }
}
```

### 2. Controller com Validação

```csharp
[ApiController]
[Route("api/[controller]")]
[TenantValidation]
public class ClienteController : BaseCrudController<Cliente, ClienteDTO, CreateClienteDTO, UpdateClienteDTO>
{
    public ClienteController(IClienteService clienteService) 
        : base(clienteService)
    {
    }
}
```

## Considerações de Performance

### 1. Índices no Banco de Dados

```sql
-- Criar índice composto para otimizar consultas por tenant
CREATE INDEX IX_Clientes_TenantId_Id ON Clientes (TenantId, Id);
CREATE INDEX IX_Reservas_TenantId_Data ON Reservas (TenantId, Data);
```

### 2. Cache por Tenant

```csharp
public class TenantAwareCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ITenantService _tenantService;

    public TenantAwareCacheService(IDistributedCache cache, ITenantService tenantService)
    {
        _cache = cache;
        _tenantService = tenantService;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        var tenantKey = $"{tenantId}_{key}";
        
        var value = await _cache.GetStringAsync(tenantKey);
        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        var tenantKey = $"{tenantId}_{key}";
        
        var jsonValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(tenantKey, jsonValue, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
    }
}
```

## Migração de Dados

### 1. Script de Migração

```sql
-- Adicionar coluna TenantId em todas as tabelas
ALTER TABLE Clientes ADD COLUMN TenantId INT NOT NULL DEFAULT 1;
ALTER TABLE Reservas ADD COLUMN TenantId INT NOT NULL DEFAULT 1;
ALTER TABLE Locais ADD COLUMN TenantId INT NOT NULL DEFAULT 1;
-- ... outras tabelas

-- Criar índices
CREATE INDEX IX_Clientes_TenantId ON Clientes (TenantId);
CREATE INDEX IX_Reservas_TenantId ON Reservas (TenantId);
CREATE INDEX IX_Locais_TenantId ON Locais (TenantId);
-- ... outros índices
```

### 2. Migration Entity Framework

```csharp
public partial class AddTenantIdToEntities : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TenantId",
            table: "Clientes",
            type: "integer",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<int>(
            name: "TenantId",
            table: "Reservas",
            type: "integer",
            nullable: false,
            defaultValue: 1);

        // ... outras tabelas

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_TenantId",
            table: "Clientes",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Reservas_TenantId",
            table: "Reservas",
            column: "TenantId");

        // ... outros índices
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Clientes_TenantId",
            table: "Clientes");

        migrationBuilder.DropIndex(
            name: "IX_Reservas_TenantId",
            table: "Reservas");

        // ... outros índices

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Clientes");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Reservas");

        // ... outras colunas
    }
}
```

## Testes

### 1. Teste de Repositório

```csharp
[Test]
public async Task GetAllAsync_ShouldReturnOnlyTenantData()
{
    // Arrange
    var tenantId = 123;
    var tenantService = new Mock<ITenantService>();
    tenantService.Setup(x => x.GetCurrentTenant()).Returns(tenantId);

    var context = CreateTestContext();
    var repository = new ClienteRepository(context, tenantService.Object);

    // Criar dados de diferentes tenants
    await context.Clientes.AddRangeAsync(
        new Cliente { Id = Guid.NewGuid(), TenantId = tenantId, Nome = "Cliente 1" },
        new Cliente { Id = Guid.NewGuid(), TenantId = tenantId, Nome = "Cliente 2" },
        new Cliente { Id = Guid.NewGuid(), TenantId = 456, Nome = "Cliente Outro Tenant" }
    );
    await context.SaveChangesAsync();

    // Act
    var result = await repository.GetAllAsync();

    // Assert
    Assert.That(result.Count(), Is.EqualTo(2));
    Assert.That(result.All(c => c.TenantId == tenantId), Is.True);
}
```

### 2. Teste de Middleware

```csharp
[Test]
public async Task TenantMiddleware_ShouldSetTenantFromHeader()
{
    // Arrange
    var tenantService = new Mock<ITenantService>();
    var middleware = new TenantMiddleware(next: (context) => Task.CompletedTask);

    var context = new DefaultHttpContext();
    context.Request.Headers["X-Tenant-ID"] = "123";

    // Act
    await middleware.InvokeAsync(context, tenantService.Object);

    // Assert
    tenantService.Verify(x => x.SetCurrentTenant(123), Times.Once);
}
```

## Benefícios da Arquitetura Multitenant

### 1. Isolamento de Dados
- Cada tenant tem acesso apenas aos seus próprios dados
- Segurança garantida em nível de aplicação e banco de dados

### 2. Escalabilidade
- Múltiplas empresas podem usar a mesma instância
- Redução de custos de infraestrutura

### 3. Manutenibilidade
- Código único para todos os tenants
- Atualizações aplicadas a todos simultaneamente

### 4. Flexibilidade
- Configurações específicas por tenant
- Personalização de funcionalidades

## Considerações de Deploy

### 1. Variáveis de Ambiente
```bash
# Configuração por ambiente
ASPNETCORE_ENVIRONMENT=Production
TENANT_DEFAULT_ID=1
TENANT_HEADER_NAME=X-Tenant-ID
```

### 2. Configuração de DNS
```
# Configurar subdomains para cada tenant
empresa1.ludusgestao.com -> A 192.168.1.100
empresa2.ludusgestao.com -> A 192.168.1.100
```

### 3. Monitoramento
- Logs separados por tenant
- Métricas de performance por tenant
- Alertas específicos por tenant
