using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Auth;
using LudusGestao.Application.Common.Models;
using LudusGestao.Application.Services;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("entrar")]
    public async Task<IActionResult> Entrar([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.Success)
            return Unauthorized(result);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO dto)
    {
        var result = await _authService.RefreshAsync(dto);
        if (!result.Success)
            return Unauthorized(result);
        return Ok(result);
    }
} 