using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Local;
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
[Route("api/locais")]
[Authorize]
public class LocaisController : BaseCrudController<LocalService, LocalDTO, CreateLocalDTO, UpdateLocalDTO>
{
    public LocaisController(LocalService service) : base(service) { }

    [HttpGet("resumo")]
    public async Task<IActionResult> ObterResumo()
    {
        try
        {
            var locais = await _service.Listar();

            var resumo = new LocalResumoDTO
            {
                TotalLocais = locais.Count(),
                Ativos = locais.Count(l => l.Situacao == SituacaoLocal.Ativo),
                Inativos = locais.Count(l => l.Situacao == SituacaoLocal.Inativo),
                Manutencao = locais.Count(l => l.Situacao == SituacaoLocal.Manutencao),
                ValorMedioHora = locais.Any() ? locais.Average(l => l.ValorHora) : 0
            };

            return Ok(new ApiResponse<LocalResumoDTO>(resumo, "Resumo de locais obtido com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter resumo de locais" });
        }
    }
} 