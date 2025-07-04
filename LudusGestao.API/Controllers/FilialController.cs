using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.Services;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/filiais")]
[Authorize]
public class FiliaisController : BaseCrudController<FilialService, FilialDTO, CreateFilialDTO, UpdateFilialDTO>
{
    public FiliaisController(FilialService service) : base(service) { }
} 