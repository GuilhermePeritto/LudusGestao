using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.Services;
using System;
using System.Threading.Tasks;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : BaseCrudController<ClienteService, ClienteDTO, CreateClienteDTO, UpdateClienteDTO>
{
    public ClientesController(ClienteService service) : base(service) { }
} 