using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Application.DTOs.Gerencialmento;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LudusGestao.API.Controllers;
[ApiController]
[Route("api/gerencialmento")]
[Authorize]
public class GerencialmentoController : ControllerBase
{
    private readonly IGerencialmentoService _gerencialmentoService;

    public GerencialmentoController(IGerencialmentoService gerencialmentoService)
    {
        _gerencialmentoService = gerencialmentoService;
    }

    [HttpPost("novo-cliente")]
    public async Task<IActionResult> CriarNovoCliente([FromBody] CriarNovoClienteDTO dto)
    {
        try
        {
            // Só permite TenantId = 1
            var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (tenantIdClaim == null || tenantIdClaim != "1")
                return StatusCode(403, new { Success = false, Message = "Acesso restrito ao tenant master" });

            var resultado = await _gerencialmentoService.CriarNovoCliente(dto);
            
            if (resultado.Success)
                return Ok(resultado);
            else
                return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "Erro interno do servidor" });
        }
    }

    [HttpPost("alterar-senha")]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO dto)
    {
        try
        {
            // Só permite TenantId = 1
            var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            if (tenantIdClaim == null || tenantIdClaim != "1")
                return StatusCode(403, new { Success = false, Message = "Acesso restrito ao tenant master" });

            var resultado = await _gerencialmentoService.AlterarSenha(dto);
            
            if (resultado.Success)
                return Ok(resultado);
            else
                return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "Erro interno do servidor" });
        }
    }
} 