# Estrutura de Situações - Ludus Gestão

## Visão Geral

O sistema utiliza uma estrutura hierárquica de situações para gerenciar o estado dos diferentes tipos de entidades. Cada categoria tem suas próprias situações específicas, mas algumas seguem um padrão comum.

## Categorias de Situações

### 1. Cadastros (Padrão)
Situações padrão aplicadas a entidades básicas do sistema.

```csharp
public enum SituacaoBase
{
    Ativo = 1,    // Registro em uso
    Inativo = 2   // Registro desabilitado
}
```

**Aplicado em:**
- Empresas
- Filiais
- Usuários

### 2. Recebíveis
Situações específicas para títulos a receber.

```csharp
public enum SituacaoRecebivel
{
    Aberto = 1,        // Título ainda não pago
    Vencido = 2,       // Passou da data de vencimento
    Pago = 3,          // Pagamento confirmado
    Cancelado = 4,     // Cancelado manualmente ou por cancelamento de evento
    Estornado = 5      // Estorno manual
}
```

### 3. Pagáveis
Situações para títulos a pagar.

```csharp
public enum SituacaoPagavel
{
    Aberto = 1,        // Título ainda não pago
    Vencido = 2,       // Passou da data de vencimento
    Cancelado = 3      // Cancelado
}
```

### 4. Eventos (Reservas)
Situações para reservas e eventos.

```csharp
public enum SituacaoReserva
{
    Confirmado = 1,    // Cliente pagou
    Concluido = 2,     // Pós término do evento
    Pendente = 3,      // Confirmação pendente (ainda não pagou)
    Cancelado = 4      // Cancelado
}
```

### 5. Clientes
Situações específicas para clientes.

```csharp
public enum SituacaoCliente
{
    Ativo = SituacaoBase.Ativo,
    Inativo = SituacaoBase.Inativo,
    Bloqueado = 3      // Cliente com títulos vencidos
}
```

### 6. Locais
Situações para locais de reserva.

```csharp
public enum SituacaoLocal
{
    Ativo = SituacaoBase.Ativo,
    Inativo = SituacaoBase.Inativo,
    Manutencao = 3     // Em manutenção
}
```

## Fluxo de Situações

### Recebíveis
```
Aberto → Pago
   ↓
Vencido → Cancelado
   ↓
Estornado
```

### Reservas
```
Pendente → Confirmado → Concluido
    ↓
Cancelado
```

### Clientes
```
Ativo → Bloqueado (automático quando há títulos vencidos)
  ↓
Inativo
```

## Implementação

### Nas Entidades
```csharp
public class Reserva : BaseEntity, ITenantEntity
{
    public SituacaoReserva Situacao { get; set; }
    // ... outras propriedades
}
```

### Nos DTOs
```csharp
public class ReservaDTO
{
    public SituacaoReserva Situacao { get; set; }
    // ... outras propriedades
}
```

### Validações
```csharp
public class CreateReservaValidator : AbstractValidator<CreateReservaDTO>
{
    public CreateReservaValidator()
    {
        RuleFor(x => x.Situacao)
            .IsInEnum()
            .WithMessage("Situação inválida");
    }
}
```

## Transições de Estado

### Automáticas
- **Cliente → Bloqueado**: Quando há títulos vencidos
- **Recebível → Vencido**: Quando passa da data de vencimento
- **Reserva → Concluido**: Após o término do evento

### Manuais
- **Recebível → Cancelado**: Cancelamento manual
- **Recebível → Estornado**: Estorno manual
- **Reserva → Cancelado**: Cancelamento de reserva

## Considerações de Negócio

1. **Cliente Bloqueado**: Não pode fazer novas reservas
2. **Local em Manutenção**: Não disponível para reservas
3. **Recebível Vencido**: Gera bloqueio automático do cliente
4. **Reserva Pendente**: Pode ser cancelada sem custo
5. **Reserva Confirmada**: Requer pagamento para cancelamento

## Extensibilidade

Para adicionar novas situações:

1. Adicionar o valor no enum correspondente
2. Atualizar a documentação
3. Implementar lógica de transição se necessário
4. Atualizar validações
5. Atualizar testes 