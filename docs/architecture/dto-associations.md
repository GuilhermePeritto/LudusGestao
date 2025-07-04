# DTOs com Associações - Ludus Gestão

## Visão Geral

Os DTOs (Data Transfer Objects) do sistema Ludus Gestão incluem associações completas para fornecer dados ricos e contextualizados nas operações de listagem e consulta. Isso elimina a necessidade de múltiplas chamadas à API para obter informações relacionadas.

## Estrutura de Associações

### 1. ReservaDTO

Representa uma reserva com todas as suas associações.

```csharp
public class ReservaDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    // Associações
    public Guid ClienteId { get; set; }
    public ClienteDTO Cliente { get; set; }
    public Guid LocalId { get; set; }
    public LocalDTO Local { get; set; }
    public Guid? UsuarioId { get; set; }
    public UsuarioDTO Usuario { get; set; }
    
    // Dados da reserva
    public DateTime Data { get; set; }
    public string HoraInicio { get; set; }
    public string HoraFim { get; set; }
    public StatusReserva Status { get; set; }
    public string Cor { get; set; }
    public string Esporte { get; set; }
    public string Observacoes { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

**Associações incluídas:**
- **Cliente**: Dados completos do cliente que fez a reserva
- **Local**: Dados completos do local reservado
- **Usuário**: Dados do usuário que processou a reserva (opcional)

### 2. RecebivelDTO

Representa um título a receber com suas associações.

```csharp
public class RecebivelDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    // Associações
    public Guid ClienteId { get; set; }
    public ClienteDTO Cliente { get; set; }
    public Guid? ReservaId { get; set; }
    public ReservaDTO Reserva { get; set; }
    
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public StatusRecebivel Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

**Associações incluídas:**
- **Cliente**: Dados completos do cliente devedor
- **Reserva**: Dados da reserva relacionada (opcional)

### 3. LocalDTO

Representa um local com suas associações.

```csharp
public class LocalDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Subtitulo { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public int Intervalo { get; set; }
    public decimal ValorHora { get; set; }
    public int? Capacidade { get; set; }
    public string Descricao { get; set; }
    public List<string> Comodidades { get; set; } = new List<string>();
    public StatusLocal Status { get; set; }
    public string Cor { get; set; }
    public string HoraAbertura { get; set; }
    public string HoraFechamento { get; set; }
    
    // Associações
    public Guid FilialId { get; set; }
    public FilialDTO Filial { get; set; }
    
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

**Associações incluídas:**
- **Filial**: Dados completos da filial onde o local está localizado

### 4. ClienteDTO

Representa um cliente com dados básicos.

```csharp
public class ClienteDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Subtitulo { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public string Observacoes { get; set; }
    public StatusCliente Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

### 5. FilialDTO

Representa uma filial com suas associações.

```csharp
public class FilialDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Nome { get; set; }
    public string Endereco { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public StatusBase Status { get; set; }
    
    // Associações
    public Guid EmpresaId { get; set; }
    public EmpresaDTO Empresa { get; set; }
    
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

**Associações incluídas:**
- **Empresa**: Dados completos da empresa proprietária da filial

### 6. EmpresaDTO

Representa uma empresa com dados básicos.

```csharp
public class EmpresaDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public string Endereco { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public StatusBase Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

### 7. UsuarioDTO

Representa um usuário do sistema.

```csharp
public class UsuarioDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public StatusBase Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TenantId { get; set; }
}
```

## Implementação no AutoMapper

### MappingProfile

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos básicos
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<Empresa, EmpresaDTO>().ReverseMap();
        CreateMap<Filial, FilialDTO>().ReverseMap();
        CreateMap<Local, LocalDTO>().ReverseMap();
        CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        
        // Mapeamentos com associações
        CreateMap<Reserva, ReservaDTO>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
            .ForMember(dest => dest.Local, opt => opt.MapFrom(src => src.Local))
            .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario));
            
        CreateMap<Recebivel, RecebivelDTO>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
            .ForMember(dest => dest.Reserva, opt => opt.MapFrom(src => src.Reserva));
            
        CreateMap<Local, LocalDTO>()
            .ForMember(dest => dest.Filial, opt => opt.MapFrom(src => src.Filial));
            
        CreateMap<Filial, FilialDTO>()
            .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa));
    }
}
```

## Implementação nos Repositórios

### Exemplo: ReservaRepository

```csharp
public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
{
    public ReservaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reserva>> GetAllWithAssociationsAsync()
    {
        return await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Local)
                .ThenInclude(l => l.Filial)
                    .ThenInclude(f => f.Empresa)
            .Include(r => r.Usuario)
            .ToListAsync();
    }

    public async Task<Reserva> GetByIdWithAssociationsAsync(Guid id)
    {
        return await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Local)
                .ThenInclude(l => l.Filial)
                    .ThenInclude(f => f.Empresa)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
```

## Implementação nos Services

### Exemplo: ReservaService

```csharp
public class ReservaService : BaseCrudService<Reserva, ReservaDTO, CreateReservaDTO, UpdateReservaDTO>, IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IMapper _mapper;

    public ReservaService(IReservaRepository reservaRepository, IMapper mapper) 
        : base(reservaRepository, mapper)
    {
        _reservaRepository = reservaRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ReservaDTO>>> GetAllWithAssociationsAsync()
    {
        try
        {
            var reservas = await _reservaRepository.GetAllWithAssociationsAsync();
            var reservasDTO = _mapper.Map<IEnumerable<ReservaDTO>>(reservas);
            
            return ApiResponse<IEnumerable<ReservaDTO>>.Success(reservasDTO);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ReservaDTO>>.Error($"Erro ao buscar reservas: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ReservaDTO>> GetByIdWithAssociationsAsync(Guid id)
    {
        try
        {
            var reserva = await _reservaRepository.GetByIdWithAssociationsAsync(id);
            if (reserva == null)
                return ApiResponse<ReservaDTO>.Error("Reserva não encontrada");

            var reservaDTO = _mapper.Map<ReservaDTO>(reserva);
            return ApiResponse<ReservaDTO>.Success(reservaDTO);
        }
        catch (Exception ex)
        {
            return ApiResponse<ReservaDTO>.Error($"Erro ao buscar reserva: {ex.Message}");
        }
    }
}
```

## Benefícios das Associações

### 1. Performance
- **Redução de chamadas**: Uma única chamada retorna dados completos
- **Menos round-trips**: Reduz latência de rede
- **Cache eficiente**: Dados relacionados podem ser cacheados juntos

### 2. Experiência do Desenvolvedor
- **Dados completos**: Frontend recebe todas as informações necessárias
- **Menos complexidade**: Não precisa gerenciar múltiplas chamadas
- **Consistência**: Dados sempre sincronizados

### 3. Flexibilidade
- **Consultas ricas**: Possibilidade de filtrar por dados relacionados
- **Ordenação**: Ordenar por campos das associações
- **Agregação**: Calcular métricas baseadas em associações

## Considerações de Performance

### 1. Lazy Loading vs Eager Loading
```csharp
// Eager Loading (recomendado para DTOs)
var reservas = await _context.Reservas
    .Include(r => r.Cliente)
    .Include(r => r.Local)
    .ToListAsync();

// Lazy Loading (pode causar N+1 queries)
var reservas = await _context.Reservas.ToListAsync();
foreach (var reserva in reservas)
{
    var cliente = reserva.Cliente; // Query adicional
}
```

### 2. Paginação com Associações
```csharp
public async Task<ApiPagedResponse<ReservaDTO>> GetPagedWithAssociationsAsync(
    int page, int pageSize, string search = null)
{
    var query = _context.Reservas
        .Include(r => r.Cliente)
        .Include(r => r.Local)
        .Include(r => r.Usuario);

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r => 
            r.Cliente.Nome.Contains(search) || 
            r.Local.Nome.Contains(search));
    }

    var total = await query.CountAsync();
    var reservas = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var reservasDTO = _mapper.Map<IEnumerable<ReservaDTO>>(reservas);
    
    return new ApiPagedResponse<ReservaDTO>
    {
        Data = reservasDTO,
        Total = total,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int)Math.Ceiling((double)total / pageSize)
    };
}
```

### 3. Otimização de Queries
```csharp
// Usar Select para projetar apenas campos necessários
var reservas = await _context.Reservas
    .Select(r => new ReservaDTO
    {
        Id = r.Id,
        Cliente = new ClienteDTO 
        { 
            Id = r.Cliente.Id, 
            Nome = r.Cliente.Nome 
        },
        Local = new LocalDTO 
        { 
            Id = r.Local.Id, 
            Nome = r.Local.Nome 
        }
    })
    .ToListAsync();
```

## Extensibilidade

Para adicionar novas associações:

1. **Atualizar entidade**: Adicionar propriedade de navegação
2. **Atualizar DTO**: Adicionar propriedade correspondente
3. **Atualizar AutoMapper**: Configurar mapeamento
4. **Atualizar repositório**: Incluir Include() necessário
5. **Atualizar testes**: Adicionar testes para nova associação 