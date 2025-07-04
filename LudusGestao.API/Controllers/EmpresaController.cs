using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Application.Services;
using System;
using System.Threading.Tasks;
using LudusGestao.API.Controllers;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/empresas")]
[Authorize]
public class EmpresasController : BaseCrudController<EmpresaService, EmpresaDTO, CreateEmpresaDTO, UpdateEmpresaDTO>
{
    public EmpresasController(EmpresaService service) : base(service) { }
} 