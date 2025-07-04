# Ludus Gestão

Sistema de gestão para locais esportivos e reservas, desenvolvido em .NET 8 com arquitetura limpa.

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture com as seguintes camadas:

- **API**: Controllers e configuração da aplicação
- **Application**: Casos de uso, DTOs, validações e mapeamentos
- **Domain**: Entidades, Value Objects, interfaces e regras de negócio
- **Infrastructure**: Implementação de repositórios, contexto do banco e serviços externos

## 🚀 Como Executar

### Pré-requisitos

- .NET 8.0 SDK
- Docker Desktop
- PostgreSQL (opcional, se não usar Docker)

### Opção 1: Com Docker (Recomendado)

#### Windows (PowerShell)
```powershell
# Iniciar
.\scripts\docker\start.ps1

# Parar
.\scripts\docker\stop.ps1
```

#### Linux/macOS (Bash)
```bash
# Dar permissão de execução
chmod +x scripts/docker/start.sh
chmod +x scripts/docker/stop.sh

# Iniciar
./scripts/docker/start.sh

# Parar
./scripts/docker/stop.sh
```

#### Manual
```bash
cd docker
docker-compose up --build -d
```

### Opção 2: Local

1. **Configurar banco de dados**
   ```bash
   # Criar banco PostgreSQL
   createdb ludusdb_dev
   ```

2. **Configurar connection string**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=ludusdb_dev;Username=seu_usuario;Password=sua_senha"
     }
   }
   ```

3. **Executar migrações**
   ```bash
   cd LudusGestao.API
   dotnet ef database update
   ```

4. **Executar a aplicação**
   ```bash
   dotnet run
   ```

## 📊 Estrutura de Situações

O sistema utiliza uma estrutura hierárquica de situações:

### Cadastros (Padrão)
- **Ativo**: Registro em uso
- **Inativo**: Registro desabilitado

### Recebíveis
- **Aberto**: Título ainda não pago
- **Vencido**: Passou da data de vencimento
- **Pago**: Pagamento confirmado
- **Cancelado**: Cancelado manualmente ou por cancelamento de evento
- **Estornado**: Estorno manual

### Pagáveis
- **Aberto**: Título ainda não pago
- **Vencido**: Passou da data de vencimento
- **Cancelado**: Cancelado

### Eventos (Reservas)
- **Confirmado**: Cliente pagou
- **Concluído**: Pós término do evento
- **Pendente**: Confirmação pendente (ainda não pagou)
- **Cancelado**: Cancelado

### Clientes
- **Ativo**: Cliente ativo
- **Inativo**: Cliente inativo
- **Bloqueado**: Cliente com títulos vencidos

### Locais
- **Ativo**: Local disponível
- **Inativo**: Local desabilitado
- **Manutenção**: Em manutenção

## 🔧 Value Objects

O sistema inclui Value Objects com validação:

### Documento
- Validação de CPF e CNPJ
- Formatação automática
- Detecção automática do tipo

### Email
- Validação de formato
- Normalização (lowercase)

### Telefone
- Validação de formato brasileiro
- Suporte a fixo e celular
- Formatação automática

## 📝 DTOs com Associações

Os DTOs de listagem incluem associações completas:

- **ReservaDTO**: Inclui Cliente, Local e Usuário
- **RecebivelDTO**: Inclui Cliente e Reserva
- **LocalDTO**: Inclui Filial
- **FilialDTO**: Inclui Empresa

## 🐳 Docker

### Serviços
- **API**: Porta 5000 (HTTP) e 5001 (HTTPS)
- **PostgreSQL**: Porta 5432

### Variáveis de Ambiente
```bash
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=postgres;Database=ludusdb_dev;Username=ludus;Password=ludus123
```

## 📚 Documentação da API

A documentação Swagger está disponível em:
- http://localhost:5000/swagger (Docker)
- https://localhost:7001/swagger (Local)

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test tests/SistemaReservas.Application.Tests/
```

## 📁 Estrutura do Projeto

```
LudusGestao/
├── LudusGestao.API/           # Camada de apresentação
├── LudusGestao.Application/   # Casos de uso e DTOs
├── LudusGestao.Domain/        # Entidades e regras de negócio
├── LudusGestao.Infrastructure/# Implementações de infraestrutura
├── docker/                    # Configurações Docker
├── docs/                      # Documentação
├── scripts/                   # Scripts de automação
└── tests/                     # Testes automatizados
```

## 🔄 Migrações

```bash
# Criar nova migração
dotnet ef migrations add NomeDaMigracao --project LudusGestao.Infrastructure --startup-project LudusGestao.API

# Aplicar migrações
dotnet ef database update --project LudusGestao.Infrastructure --startup-project LudusGestao.API
```

## 📞 Suporte

Para dúvidas ou problemas, consulte a documentação ou entre em contato com a equipe de desenvolvimento. 