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