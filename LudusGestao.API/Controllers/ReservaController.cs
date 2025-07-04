using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Reserva;
using LudusGestao.Application.Services;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/reservas")]
[Authorize]
public class ReservasController : BaseCrudController<ReservaService, ReservaDTO, CreateReservaDTO, UpdateReservaDTO>
{
    public ReservasController(ReservaService service) : base(service) { }
} 