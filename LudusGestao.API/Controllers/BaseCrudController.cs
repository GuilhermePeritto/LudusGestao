using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.Common.Models;
using System;
using System.Threading.Tasks;
using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Domain.Common;
using LudusGestao.Domain.Common.Exceptions;

namespace LudusGestao.API.Controllers
{
    [ApiController]
    public abstract class BaseCrudController<TService, TDto, TCreateDto, TUpdateDto> : ControllerBase
        where TService : IBaseCrudService<TDto, TCreateDto, TUpdateDto>
    {
        protected readonly TService _service;

        protected BaseCrudController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Listar([FromQuery] QueryParamsBase queryParams)
        {
            var paged = await _service.ListarPaginado(queryParams);
            return Ok(paged);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var dto = await _service.ObterPorId(id);
                if (dto == null) return NotFoundResponse();
                return Ok(new ApiResponse<TDto>(dto));
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Criar([FromBody] TCreateDto dto)
        {
            try
            {
                var created = await _service.Criar(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = GetId(created) }, new ApiResponse<TDto>(created));
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Atualizar(Guid id, [FromBody] TUpdateDto dto)
        {
            try
            {
                var updated = await _service.Atualizar(id, dto);
                if (updated == null) return NotFoundResponse();
                return Ok(new ApiResponse<TDto>(updated));
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                var sucesso = await _service.Remover(id);
                if (!sucesso) return NotFoundResponse();
                return NoContent();
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }

        // Helper para obter o Id do DTO criado
        protected virtual Guid GetId(TDto dto)
        {
            var prop = typeof(TDto).GetProperty("Id");
            return prop != null ? (Guid)prop.GetValue(dto) : Guid.Empty;
        }

        [NonAction]
        protected IActionResult ErrorResponse(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new ApiResponse<object>(default) { Success = false, Message = message });
        }

        [NonAction]
        protected IActionResult NotFoundResponse(string message = "Recurso não encontrado.")
        {
            return NotFound(new ApiResponse<object>(default) { Success = false, Message = message });
        }

        [NonAction]
        protected IActionResult ExceptionResponse(Exception ex)
        {
            var message = ex.Message;
            var status = 400;
            if (ex is LudusGestao.Domain.Common.Exceptions.ValidationException)
                status = 400;
            else if (ex is LudusGestao.Domain.Common.Exceptions.NotFoundException)
                status = 404;
            else if (ex is LudusGestao.Domain.Common.Exceptions.DomainException)
                status = 400;
            else
                status = 500;
            return StatusCode(status, new ApiResponse<object>(default) { Success = false, Message = message });
        }
    }
} 