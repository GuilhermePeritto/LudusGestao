using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Local;
using LudusGestao.Application.Services;
using System;
using System.Threading.Tasks;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/locais")]
[Authorize]
public class LocaisController : BaseCrudController<LocalService, LocalDTO, CreateLocalDTO, UpdateLocalDTO>
{
    public LocaisController(LocalService service) : base(service) { }
} 