using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.DTOs.Utilitarios;
using LudusGestao.Application.Services;
using LudusGestao.Application.Common.Models;
using LudusGestao.Domain.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : BaseCrudController<ClienteService, ClienteDTO, CreateClienteDTO, UpdateClienteDTO>
{
    public ClientesController(ClienteService service) : base(service) { }

    [HttpGet("resumo")]
    public async Task<IActionResult> ObterResumo()
    {
        try
        {
            var clientes = await _service.Listar();
            var dataAtual = DateTime.UtcNow;

            var resumo = new ClienteResumoDTO
            {
                TotalClientes = clientes.Count(),
                Ativos = clientes.Count(c => c.Situacao == SituacaoCliente.Ativo.ToString()),
                Inativos = clientes.Count(c => c.Situacao == SituacaoCliente.Inativo.ToString()),
                NovosMes = clientes.Count(c => 
                    c.DataCriacao.Year == dataAtual.Year && 
                    c.DataCriacao.Month == dataAtual.Month),
                PessoaJuridica = clientes.Count(c => c.Documento.Length > 11)
            };

            return Ok(new ApiResponse<ClienteResumoDTO>(resumo, "Resumo de clientes obtido com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter resumo de clientes" });
        }
    }
} 