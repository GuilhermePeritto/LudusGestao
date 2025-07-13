# Script para executar o seed dos dados base
# Executa uma requisição POST para o endpoint /api/utilitarios/seed

param(
    [string]$BaseUrl = "http://localhost:5000"
)

Write-Host "Executando seed dos dados base..." -ForegroundColor Yellow

try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/utilitarios/seed" -Method POST -ContentType "application/json"
    Write-Host "✅ $($response.message)" -ForegroundColor Green
}
catch {
    Write-Host "❌ Erro ao executar seed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Detalhes: $responseBody" -ForegroundColor Red
    }
}

Write-Host "`nDados inseridos:" -ForegroundColor Cyan
Write-Host "- Empresa: Ludus Sistemas" -ForegroundColor White
Write-Host "- Filial: Filial Central" -ForegroundColor White
Write-Host "- Grupo de Permissão: Administrador" -ForegroundColor White
Write-Host "- Permissão: Gerenciar Usuários" -ForegroundColor White
Write-Host "- Usuário: Admin Ludus (admin@ludus.com.br / Ludus@2024)" -ForegroundColor White
Write-Host "- Cliente: Cliente Exemplo" -ForegroundColor White
Write-Host "- Local: Quadra Exemplo" -ForegroundColor White
Write-Host "- Reserva: Reserva teste" -ForegroundColor White
Write-Host "- Recebível: Recebimento teste" -ForegroundColor White 