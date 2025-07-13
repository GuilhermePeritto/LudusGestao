using LudusGestao.Application.Common.Models;
using System;
using System.Threading.Tasks;
using LudusGestao.Domain.Common;

namespace LudusGestao.Application.Common.Interfaces
{
    public interface IBaseCrudService<TDto, TCreateDto, TUpdateDto>
    {
        Task<ApiPagedResponse<TDto>> ListarPaginado(QueryParamsBase queryParams);
        Task<IEnumerable<TDto>> Listar();
        Task<TDto> ObterPorId(Guid id);
        Task<TDto> Criar(TCreateDto dto);
        Task<TDto> Atualizar(Guid id, TUpdateDto dto);
        Task<bool> Remover(Guid id);
    }
} 