using Microsoft.AspNetCore.Http;
using LudusGestao.Domain.Interfaces.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;

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
            if (path.StartsWith("/api/autenticacao/") ||
                path.StartsWith("/api/auth/") ||
                path.StartsWith("/api/utilitarios/"))
            {
                await _next(context);
                return;
            }

            // Verificar se o usuário está autenticado
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    success = false,
                    message = "Token de autenticação não fornecido ou inválido.",
                    statusCode = 401
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;
            }

            // Extrai o TenantId do claim do token JWT
            var tenantIdStr = context.User?.Claims?.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdStr) || !int.TryParse(tenantIdStr, out var tenantId))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    success = false,
                    message = "TenantId não informado ou inválido no token.",
                    statusCode = 401
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;
            }

            tenantService.SetTenant(tenantId.ToString());
            await _next(context);
        }
    }
} 