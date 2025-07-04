using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Recebivel;
using LudusGestao.Application.Services;
using System;
using System.Threading.Tasks;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/recebiveis")]
[Authorize]
public class RecebiveisController : BaseCrudController<RecebivelService, RecebivelDTO, CreateRecebivelDTO, UpdateRecebivelDTO>
{
    public RecebiveisController(RecebivelService service) : base(service) { }
} 