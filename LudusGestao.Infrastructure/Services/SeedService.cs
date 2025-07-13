using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Enums;
using LudusGestao.Domain.Interfaces.Services;
using LudusGestao.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Infrastructure.Services
{
    public class SeedService : ISeedService
    {
        private readonly ApplicationDbContext _context;

        public SeedService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SeedDadosBaseAsync()
        {
            try
            {
                // Verificar se já existe dados (ignorando filtro de tenant)
                if (await _context.Empresas.IgnoreQueryFilters().AnyAsync())
                {
                    return false; // Já existe dados
                }

                // 1. Empresa
                var empresaLudus = new Empresa
                {
                    Nome = "Ludus Gestão",
                    Cnpj = "12.345.678/0001-90",
                    Email = "contato@ludusgestao.com.br",
                    Endereco = "Rua das Empresas, 123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Cep = "01234-567",
                    Telefone = "(11) 2222-2222",
                    Situacao = SituacaoBase.Ativo,
                    TenantId = 1
                };
                _context.Empresas.Add(empresaLudus);
                await _context.SaveChangesForSeedAsync();

                // 2. Filial
                var filial = new Filial
                {
                    Nome = "Filial Matriz",
                    Codigo = "MATRIZ",
                    Endereco = "Rua das Filiais, 456",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Cep = "01234-567",
                    Telefone = "(11) 3333-3333",
                    Email = "filial@ludusgestao.com.br",
                    Cnpj = "12.345.678/0001-91",
                    Responsavel = "João Silva",
                    DataAbertura = DateTime.UtcNow.AddYears(-1),
                    Situacao = SituacaoBase.Ativo,
                    TenantId = 1,
                    EmpresaId = empresaLudus.Id
                };
                _context.Filiais.Add(filial);
                await _context.SaveChangesForSeedAsync();

                // 3. Cliente
                var cliente = new Cliente
                {
                    Nome = "Cliente Exemplo",
                    Documento = "123.456.789-00",
                    Email = "cliente@exemplo.com",
                    Telefone = "(11) 90000-0000",
                    Endereco = "Rua dos Clientes, 789, Bairro, São Paulo, SP, 01234-567",
                    Observacoes = "Cliente de teste",
                    Situacao = LudusGestao.Domain.Enums.SituacaoCliente.Ativo,
                    TenantId = 1
                };
                _context.Clientes.Add(cliente);
                await _context.SaveChangesForSeedAsync();

                // 4. Local
                var local = new Local
                {
                    Nome = "Sala de Reunião",
                    Tipo = "Reunião",
                    Intervalo = 60,
                    ValorHora = 100,
                    Capacidade = 10,
                    Descricao = "Sala equipada",
                    Comodidades = new List<string> { "Projetor", "Ar-condicionado" },
                    Situacao = LudusGestao.Domain.Enums.SituacaoLocal.Ativo,
                    Cor = "#FF0000",
                    HoraAbertura = "08:00",
                    HoraFechamento = "18:00",
                    FilialId = filial.Id,
                    TenantId = 1
                };
                _context.Locais.Add(local);
                await _context.SaveChangesForSeedAsync();

                // 5. Permissão
                var permissao = new Permissao
                {
                    Nome = "Sistema.Acesso",
                    Descricao = "Acesso ao sistema",
                    ModuloPai = "Sistema",
                    Submodulo = "",
                    Acao = "Acessar",
                    Situacao = SituacaoBase.Ativo,
                    TenantId = 1
                };
                _context.Permissoes.Add(permissao);
                await _context.SaveChangesForSeedAsync();

                // 6. Grupo de Permissão
                var grupoAdmin = new GrupoPermissao
                {
                    Nome = "Administrador",
                    Descricao = "Acesso total ao sistema",
                    Situacao = SituacaoBase.Ativo,
                    TenantId = 1
                };
                _context.GruposPermissoes.Add(grupoAdmin);
                await _context.SaveChangesForSeedAsync();

                // 7. Usuário
                var usuarioAdmin = new Usuario
                {
                    Nome = "Administrador",
                    Email = "admin@ludussistemas.com.br",
                    Telefone = "(11) 99999-9999",
                    Cargo = "Administrador",
                    EmpresaId = empresaLudus.Id,
                    GrupoPermissaoId = grupoAdmin.Id,
                    Situacao = SituacaoBase.Ativo,
                    UltimoAcesso = DateTime.UtcNow,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("Ludus@2024"),
                    TenantId = 1
                };
                _context.Usuarios.Add(usuarioAdmin);
                await _context.SaveChangesForSeedAsync();

                // 8. Reserva
                var reserva = new Reserva
                {
                    ClienteId = cliente.Id,
                    LocalId = local.Id,
                    Data = DateTime.UtcNow.AddDays(1),
                    HoraInicio = "08:00",
                    HoraFim = "10:00",
                    Situacao = LudusGestao.Domain.Enums.SituacaoReserva.Confirmado,
                    Cor = "#00FF00",
                    Esporte = "Futebol",
                    Observacoes = "Reserva teste",
                    Valor = 200,
                    TenantId = 1
                };
                _context.Reservas.Add(reserva);
                await _context.SaveChangesForSeedAsync();

                // 9. Recebível
                var recebivel = new Recebivel
                {
                    ClienteId = cliente.Id,
                    Descricao = "Recebível referente à reserva de teste",
                    Valor = 100.00m,
                    DataVencimento = DateTime.UtcNow.AddDays(10),
                    ReservaId = reserva.Id,
                    Situacao = LudusGestao.Domain.Enums.SituacaoRecebivel.Aberto,
                    TenantId = 1
                };
                _context.Recebiveis.Add(recebivel);
                await _context.SaveChangesForSeedAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log do erro (em produção, usar ILogger)
                Console.WriteLine($"Erro ao executar seed: {ex.Message}");
                return false;
            }
        }
    }
} 