using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Application.Services;
using LudusGestao.Infrastructure.Data.Repositories;
using LudusGestao.Infrastructure.Data.Repositories.Base;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using LudusGestao.Application.Mappings;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Security;
using LudusGestao.Infrastructure.Security.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using LudusGestao.Domain.Entities;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LudusGestao API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1)
        }));
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// TenantService e IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITenantService, TenantService>();

// SeedService
builder.Services.AddScoped<ISeedService, LudusGestao.Infrastructure.Services.SeedService>();

// HttpClient para APIs externas
builder.Services.AddHttpClient();

// Repositórios e Services
builder.Services.AddScoped<IBaseRepository<Reserva>, BaseRepository<Reserva>>();
builder.Services.AddScoped<IBaseRepository<Cliente>, BaseRepository<Cliente>>();
builder.Services.AddScoped<IBaseRepository<Filial>, BaseRepository<Filial>>();
builder.Services.AddScoped<IBaseRepository<Local>, BaseRepository<Local>>();
builder.Services.AddScoped<IBaseRepository<Recebivel>, BaseRepository<Recebivel>>();
builder.Services.AddScoped<IBaseRepository<Empresa>, BaseRepository<Empresa>>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<FilialService>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<LocalService>();
builder.Services.AddScoped<IRecebivelRepository, RecebivelRepository>();
builder.Services.AddScoped<RecebivelService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
// Registrar AuthService da Infrastructure para a interface de domínio
builder.Services.AddScoped<LudusGestao.Domain.Interfaces.Services.IAuthService, LudusGestao.Infrastructure.Security.AuthService>();
// Registrar AuthService da Application para uso na controller
builder.Services.AddScoped<LudusGestao.Application.Services.AuthService>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<IGrupoPermissaoRepository, GrupoPermissaoRepository>();
builder.Services.AddScoped<GrupoPermissaoService>();
builder.Services.AddScoped<IPermissaoRepository, PermissaoRepository>();
builder.Services.AddScoped<PermissaoService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "SistemaReservasIssuer",
            ValidAudience = "SistemaReservasAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ludus-sistemas-chave-super-secreta-2024")),
            ClockSkew = TimeSpan.Zero
        };
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"JWT Token validated for user: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.MapHealthChecks("/health");

// Bloco para rodar apenas o seed se argumento 'seed' for passado
if (args.Length > 0 && args[0].ToLower() == "seed")
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    Console.WriteLine("Seed executado com sucesso.");
    return;
}

app.Run(); 