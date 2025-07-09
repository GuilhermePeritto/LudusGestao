# Script para limpar arquivos de cache do Windows que podem causar problemas no Docker
Write-Host "Limpando arquivos de cache do Windows..." -ForegroundColor Yellow

# Remover pastas obj e bin
Get-ChildItem -Path . -Recurse -Directory -Name "obj" | ForEach-Object {
    $path = Join-Path . $_
    Write-Host "Removendo: $path" -ForegroundColor Red
    Remove-Item -Path $path -Recurse -Force -ErrorAction SilentlyContinue
}

Get-ChildItem -Path . -Recurse -Directory -Name "bin" | ForEach-Object {
    $path = Join-Path . $_
    Write-Host "Removendo: $path" -ForegroundColor Red
    Remove-Item -Path $path -Recurse -Force -ErrorAction SilentlyContinue
}

# Remover arquivos .user
Get-ChildItem -Path . -Recurse -File -Filter "*.user" | ForEach-Object {
    Write-Host "Removendo: $($_.FullName)" -ForegroundColor Red
    Remove-Item -Path $_.FullName -Force -ErrorAction SilentlyContinue
}

# Remover pasta .vs
if (Test-Path ".vs") {
    Write-Host "Removendo: .vs" -ForegroundColor Red
    Remove-Item -Path ".vs" -Recurse -Force -ErrorAction SilentlyContinue
}

# Limpar cache do NuGet
Write-Host "Limpando cache do NuGet..." -ForegroundColor Yellow
dotnet nuget locals all --clear

Write-Host "Limpeza concluída!" -ForegroundColor Green
Write-Host "Agora você pode fazer o build do Docker sem problemas." -ForegroundColor Green 