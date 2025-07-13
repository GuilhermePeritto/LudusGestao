using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Permissao;
using LudusGestao.Application.Services;
using LudusGestao.Application.Common.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/permissoes")]
[Authorize]
public class PermissoesController : ControllerBase
{
    private readonly PermissaoService _permissaoService;

    public PermissoesController(PermissaoService permissaoService)
    {
        _permissaoService = permissaoService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var permissoes = await _permissaoService.Listar();
            return Ok(new ApiResponse<IEnumerable<PermissaoDTO>>(permissoes, "Permissões obtidas com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter permissões" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var permissao = await _permissaoService.ObterPorId(id);
            if (permissao == null)
                return NotFound(new ApiResponse<object>(default) { Success = false, Message = "Permissão não encontrada" });

            return Ok(new ApiResponse<PermissaoDTO>(permissao, "Permissão obtida com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter permissão" });
        }
    }

    [HttpGet("modulos-pai")]
    public async Task<IActionResult> ObterModulosPai()
    {
        try
        {
            var modulos = await _permissaoService.ObterModulosPai();
            return Ok(new ApiResponse<IEnumerable<string>>(modulos, "Módulos pai obtidos com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter módulos pai" });
        }
    }

    [HttpGet("submodulos")]
    public async Task<IActionResult> ObterSubmodulos()
    {
        try
        {
            var submodulos = await _permissaoService.ObterSubmodulos();
            return Ok(new ApiResponse<IEnumerable<string>>(submodulos, "Submódulos obtidos com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter submódulos" });
        }
    }

    [HttpGet("modulo-pai/{moduloPai}")]
    public async Task<IActionResult> ObterPorModuloPai(string moduloPai)
    {
        try
        {
            var permissoes = await _permissaoService.ObterPorModuloPai(moduloPai);
            return Ok(new ApiResponse<IEnumerable<PermissaoDTO>>(permissoes, "Permissões do módulo obtidas com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter permissões do módulo" });
        }
    }

    [HttpGet("submodulo/{submodulo}")]
    public async Task<IActionResult> ObterPorSubmodulo(string submodulo)
    {
        try
        {
            var permissoes = await _permissaoService.ObterPorSubmodulo(submodulo);
            return Ok(new ApiResponse<IEnumerable<PermissaoDTO>>(permissoes, "Permissões do submódulo obtidas com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao obter permissões do submódulo" });
        }
    }
} 