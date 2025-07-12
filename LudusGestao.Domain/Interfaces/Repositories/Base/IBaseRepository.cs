using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Domain.Common;

namespace LudusGestao.Domain.Interfaces.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> ObterPorId(Guid id);
        Task<(IEnumerable<T> Items, int TotalCount)> ListarPaginado(QueryParamsBase queryParams);
        Task<IEnumerable<T>> Listar();
        Task Criar(T entity);
        Task Atualizar(T entity);
        Task Remover(Guid id);
    }
} 