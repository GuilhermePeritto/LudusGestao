# Docker - Ludus Gestão

## Visão Geral

O projeto Ludus Gestão utiliza Docker para facilitar o desenvolvimento, teste e deploy da aplicação. A configuração inclui containers para a API .NET e o banco de dados PostgreSQL.

## Estrutura de Arquivos

```
docker/
├── docker-compose.yml          # Configuração principal
├── docker-compose.override.yml # Configuração de desenvolvimento
└── Dockerfile                  # Build da aplicação

scripts/docker/
├── start.sh                    # Script de inicialização (Linux/macOS)
├── stop.sh                     # Script de parada (Linux/macOS)
├── start.ps1                   # Script de inicialização (Windows)
└── stop.ps1                    # Script de parada (Windows)
```

## Serviços

### 1. PostgreSQL (Database)

**Imagem**: `postgres:15`

**Configuração**:
- **Container**: `ludus_gestao_db`
- **Porta**: `5432:5432`
- **Usuário**: `ludus`
- **Senha**: `ludus123`
- **Database**: `ludusdb` (produção) / `ludusdb_dev` (desenvolvimento)

**Variáveis de Ambiente**:
```bash
POSTGRES_USER=ludus
POSTGRES_PASSWORD=ludus123
POSTGRES_DB=ludusdb
```

**Volumes**:
- `pgdata:/var/lib/postgresql/data` - Persistência dos dados

**Health Check**:
```yaml
healthcheck:
  test: ["CMD-SHELL", "pg_isready -U ludus"]
  interval: 30s
  timeout: 10s
  retries: 3
```

### 2. API (.NET)

**Build**: Multi-stage Dockerfile

**Configuração**:
- **Container**: `ludus_gestao_api`
- **Portas**: `5000:80` (HTTP), `5001:443` (HTTPS)
- **Dependências**: PostgreSQL (com health check)

**Variáveis de Ambiente**:
```bash
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=postgres;Database=ludusdb;Username=ludus;Password=ludus123
```

## Dockerfile

### Estrutura Multi-Stage

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY LudusGestao.sln ./
COPY LudusGestao.API/LudusGestao.API.csproj ./LudusGestao.API/
COPY LudusGestao.Application/LudusGestao.Application.csproj ./LudusGestao.Application/
COPY LudusGestao.Domain/LudusGestao.Domain.csproj ./LudusGestao.Domain/
COPY LudusGestao.Infrastructure/LudusGestao.Infrastructure.csproj ./LudusGestao.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish LudusGestao.API/LudusGestao.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

# Set entry point
ENTRYPOINT ["dotnet", "LudusGestao.API.dll"]
```

### Otimizações

1. **Multi-stage build**: Reduz o tamanho final da imagem
2. **Layer caching**: Aproveita cache do Docker para builds mais rápidos
3. **Dockerignore**: Exclui arquivos desnecessários do build

## Docker Compose

### Configuração Principal (docker-compose.yml)

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15
    container_name: ludus_gestao_db
    environment:
      POSTGRES_USER: ludus
      POSTGRES_PASSWORD: ludus123
      POSTGRES_DB: ludusdb
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    networks:
      - ludus-network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ludus"]
      interval: 30s
      timeout: 10s
      retries: 3

  api:
    build:
      context: ..
      dockerfile: docker/Dockerfile
    container_name: ludus_gestao_api
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=ludusdb;Username=ludus;Password=ludus123
    depends_on:
      postgres:
        condition: service_healthy
    networks:
      - ludus-network
    restart: unless-stopped

volumes:
  pgdata:

networks:
  ludus-network:
    driver: bridge
```

### Configuração de Desenvolvimento (docker-compose.override.yml)

```yaml
version: '3.8'

services:
  postgres:
    environment:
      POSTGRES_DB: ludusdb_dev
    ports:
      - "5432:5432"

  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=ludusdb_dev;Username=ludus;Password=ludus123
    volumes:
      - ../:/src
      - /src/bin
      - /src/obj
    ports:
      - "5000:80"
      - "5001:443"
```

## Scripts de Automação

### Windows (PowerShell)

#### start.ps1
```powershell
Write-Host "🚀 Iniciando Ludus Gestão com Docker..." -ForegroundColor Green

# Verificar se o Docker está rodando
try {
    docker info | Out-Null
} catch {
    Write-Host "❌ Docker não está rodando. Por favor, inicie o Docker Desktop e tente novamente." -ForegroundColor Red
    exit 1
}

# Navegar para o diretório docker
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location "$scriptPath\..\..\docker"

# Parar containers existentes
Write-Host "🛑 Parando containers existentes..." -ForegroundColor Yellow
docker-compose down

# Construir e iniciar os containers
Write-Host "🔨 Construindo e iniciando containers..." -ForegroundColor Yellow
docker-compose up --build -d

# Aguardar um pouco para os serviços iniciarem
Write-Host "⏳ Aguardando serviços iniciarem..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Verificar status dos containers
Write-Host "📊 Status dos containers:" -ForegroundColor Cyan
docker-compose ps

Write-Host ""
Write-Host "✅ Ludus Gestão iniciado com sucesso!" -ForegroundColor Green
Write-Host "🌐 API disponível em: http://localhost:5000" -ForegroundColor Cyan
Write-Host "🗄️  Banco de dados: localhost:5432" -ForegroundColor Cyan
Write-Host ""
Write-Host "Para ver os logs: docker-compose logs -f" -ForegroundColor Gray
Write-Host "Para parar: docker-compose down" -ForegroundColor Gray
```

#### stop.ps1
```powershell
Write-Host "🛑 Parando Ludus Gestão..." -ForegroundColor Yellow

# Navegar para o diretório docker
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location "$scriptPath\..\..\docker"

# Parar containers
docker-compose down

Write-Host "✅ Containers parados com sucesso!" -ForegroundColor Green
```

### Linux/macOS (Bash)

#### start.sh
```bash
#!/bin/bash

echo "🚀 Iniciando Ludus Gestão com Docker..."

# Verificar se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker e tente novamente."
    exit 1
fi

# Navegar para o diretório docker
cd "$(dirname "$0")/../../docker"

# Parar containers existentes
echo "🛑 Parando containers existentes..."
docker-compose down

# Construir e iniciar os containers
echo "🔨 Construindo e iniciando containers..."
docker-compose up --build -d

# Aguardar um pouco para os serviços iniciarem
echo "⏳ Aguardando serviços iniciarem..."
sleep 10

# Verificar status dos containers
echo "📊 Status dos containers:"
docker-compose ps

echo ""
echo "✅ Ludus Gestão iniciado com sucesso!"
echo "🌐 API disponível em: http://localhost:5000"
echo "🗄️  Banco de dados: localhost:5432"
echo ""
echo "Para ver os logs: docker-compose logs -f"
echo "Para parar: docker-compose down"
```

#### stop.sh
```bash
#!/bin/bash

echo "🛑 Parando Ludus Gestão..."

# Navegar para o diretório docker
cd "$(dirname "$0")/../../docker"

# Parar containers
docker-compose down

echo "✅ Containers parados com sucesso!"
```

## Comandos Úteis

### Inicialização
```bash
# Usando scripts
./scripts/docker/start.sh          # Linux/macOS
.\scripts\docker\start.ps1         # Windows

# Manual
cd docker
docker-compose up --build -d
```

### Parada
```bash
# Usando scripts
./scripts/docker/stop.sh           # Linux/macOS
.\scripts\docker\stop.ps1          # Windows

# Manual
cd docker
docker-compose down
```

### Logs
```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f api
docker-compose logs -f postgres
```

### Status
```bash
# Ver status dos containers
docker-compose ps

# Ver informações detalhadas
docker-compose ps -a
```

### Rebuild
```bash
# Rebuild completo
docker-compose down
docker-compose build --no-cache
docker-compose up -d

# Rebuild de um serviço específico
docker-compose build --no-cache api
docker-compose up -d api
```

### Banco de Dados
```bash
# Acessar PostgreSQL
docker exec -it ludus_gestao_db psql -U ludus -d ludusdb_dev

# Backup
docker exec ludus_gestao_db pg_dump -U ludus ludusdb_dev > backup.sql

# Restore
docker exec -i ludus_gestao_db psql -U ludus -d ludusdb_dev < backup.sql
```

## Troubleshooting

### Problemas Comuns

1. **Porta já em uso**
   ```bash
   # Verificar portas em uso
   netstat -an | grep :5000
   netstat -an | grep :5432
   
   # Parar processo que está usando a porta
   sudo lsof -ti:5000 | xargs kill -9
   ```

2. **Container não inicia**
   ```bash
   # Ver logs detalhados
   docker-compose logs api
   
   # Verificar se o banco está saudável
   docker-compose logs postgres
   ```

3. **Problemas de permissão (Linux)**
   ```bash
   # Dar permissão de execução aos scripts
   chmod +x scripts/docker/*.sh
   ```

4. **Limpeza de containers órfãos**
   ```bash
   # Remover containers parados
   docker container prune
   
   # Remover imagens não utilizadas
   docker image prune
   
   # Limpeza completa
   docker system prune -a
   ```

### Performance

1. **Build lento**: Use `.dockerignore` para excluir arquivos desnecessários
2. **Container lento**: Verifique se há volume mounts desnecessários
3. **Memória**: Ajuste limites de memória se necessário

## Produção

Para deploy em produção:

1. **Variáveis de ambiente**: Configure variáveis de produção
2. **Secrets**: Use Docker secrets ou variáveis de ambiente seguras
3. **Volumes**: Configure volumes persistentes adequados
4. **Networks**: Configure redes seguras
5. **Health checks**: Implemente health checks robustos
6. **Logs**: Configure logging adequado
7. **Monitoring**: Implemente monitoramento 