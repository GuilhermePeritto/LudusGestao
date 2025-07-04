using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Application.Services;
using System;
using System.Threading.Tasks;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public class UsuariosController : BaseCrudController<UsuarioService, UsuarioDTO, CreateUsuarioDTO, UpdateUsuarioDTO>
{
    public UsuariosController(UsuarioService service) : base(service) { }
} 