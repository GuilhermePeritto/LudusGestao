using Microsoft.AspNetCore.Http;
using LudusGestao.Domain.Interfaces.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace LudusGestao.Infrastructure.Security.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Ignorar Swagger, HealthChecks, favicon e raiz
            if (path == "/" ||
                path.StartsWith("/swagger") ||
                path.StartsWith("/health") ||
                path == "/favicon.ico")
            {
                await _next(context);
                return;
            }

            // Ignorar rotas públicas de autenticação
            var rotasPublicas = new[] { "/api/autenticacao/entrar", "/api/autenticacao/registrar", "/api/autenticacao/refresh" };
            if (rotasPublicas.Any(r => path.StartsWith(r)))
            {
                await _next(context);
                return;
            }

            // Extrai o TenantId do claim do token JWT
            var tenantIdStr = context.User?.Claims?.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdStr) || !int.TryParse(tenantIdStr, out var tenantId))
                throw new System.UnauthorizedAccessException("TenantId não informado ou inválido no token.");
            tenantService.SetTenant(tenantId.ToString());
            await _next(context);
        }
    }
} 