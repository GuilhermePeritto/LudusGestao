using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Recebivel;
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
[Route("api/recebiveis")]
[Authorize]
public class RecebiveisController : BaseCrudController<RecebivelService, RecebivelDTO, CreateRecebivelDTO, UpdateRecebivelDTO>
{
    public RecebiveisController(RecebivelService service) : base(service) { }

    [HttpGet("resumo")]
    public async Task<IActionResult> ObterResumo()
    {
        try
        {
            var recebiveis = await _service.Listar();

            var resumo = new RecebivelResumoDTO
            {
                TotalRecebiveis = recebiveis.Count(),
                ValorTotal = recebiveis.Sum(r => r.Valor),
                ValorPendente = recebiveis.Where(r => r.Situacao == SituacaoRecebivel.Aberto).Sum(r => r.Valor),
                ValorPago = recebiveis.Where(r => r.Situacao == SituacaoRecebivel.Pago).Sum(r => r.Valor),
                ValorVencido = recebiveis.Where(r => r.Situacao == SituacaoRecebivel.Vencido).Sum(r => r.Valor),
                Pendentes = recebiveis.Count(r => r.Situacao == SituacaoRecebivel.Aberto),
                Pagos = recebiveis.Count(r => r.Situacao == SituacaoRecebivel.Pago),
                Vencidos = recebiveis.Count(r => r.Situacao == SituacaoRecebivel.Vencido)
            };

            return Ok(new ApiResponse<RecebivelResumoDTO>(resumo, "Resumo de recebíveis obtido com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter resumo de recebíveis" });
        }
    }
} 