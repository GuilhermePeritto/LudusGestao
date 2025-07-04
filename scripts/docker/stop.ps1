Write-Host "🛑 Parando Ludus Gestão..." -ForegroundColor Yellow

# Navegar para o diretório docker
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location "$scriptPath\..\..\docker"

# Parar containers
docker-compose down

Write-Host "✅ Containers parados com sucesso!" -ForegroundColor Green 