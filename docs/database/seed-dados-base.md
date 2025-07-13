# Seed de Dados Base

Este documento explica como executar o seed dos dados base no sistema Ludus Gestão.

## O que é o Seed?

O seed é um processo que insere dados iniciais no banco de dados para permitir o funcionamento básico do sistema. Ele cria:

- **Empresa**: Ludus Sistemas
- **Filial**: Filial Central
- **Grupo de Permissão**: Administrador
- **Permissão**: Gerenciar Usuários
- **Usuário**: Admin Ludus (admin@ludus.com.br / Ludus@2024)
- **Cliente**: Cliente Exemplo
- **Local**: Quadra Exemplo
- **Reserva**: Reserva teste
- **Recebível**: Recebimento teste

## Como Executar

### Opção 1: Via API (Recomendado)

1. **Inicie a API**:
   ```bash
   cd LudusGestao.API
   dotnet run
   ```

2. **Execute o script PowerShell**:
   ```powershell
   .\scripts\database\seed-dados-base.ps1
   ```

3. **Ou faça uma requisição manual**:
   ```bash
   curl -X POST http://localhost:5000/api/utilitarios/seed
   ```

### Opção 2: Via Código

```csharp
// No Program.cs ou em um controller
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.SeedDadosBase();
}
```

## Dados Criados

### Empresa
- **Nome**: Ludus Sistemas
- **CNPJ**: 12345678000199
- **Email**: contato@ludus.com.br
- **Endereço**: Rua Exemplo, 100, Centro, CidadeX, SP, 00000-000

### Filial
- **Nome**: Filial Central
- **Código**: F001
- **Responsável**: João Gerente
- **Email**: filial@ludus.com.br

### Usuário Administrador
- **Nome**: Admin Ludus
- **Email**: admin@ludus.com.br
- **Senha**: Ludus@2024
- **Cargo**: Administrador
- **Grupo**: Administrador

### Permissões
- **Gerenciar Usuários**: Permite criar, editar e remover usuários

### Dados de Exemplo
- **Cliente**: Cliente VIP (Cliente Exemplo)
- **Local**: Quadra 1 (Quadra Exemplo) - Futebol
- **Reserva**: Reserva teste (10:00-11:00)
- **Recebível**: Recebimento teste (R$ 100,00)

## Segurança

- O seed só executa se não existirem dados para o tenant 1
- Todos os dados são criados com `TenantId = 1`
- A senha do usuário admin é criptografada com BCrypt

## Observações

- Execute o seed apenas em ambientes de desenvolvimento/teste
- Para produção, considere usar migrations ou scripts SQL específicos
- O seed pode ser executado múltiplas vezes sem duplicar dados 