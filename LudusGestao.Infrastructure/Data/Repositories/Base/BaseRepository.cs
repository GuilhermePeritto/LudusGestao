using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Domain.Common;

namespace LudusGestao.Infrastructure.Data.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> ObterPorId(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> ListarPaginado(QueryParamsBase queryParams)
        {
            var query = _context.Set<T>().AsQueryable();

            // Filtro dinâmico (simples, pode ser expandido)
            if (!string.IsNullOrEmpty(queryParams.Filter))
            {
                // Exemplo: filter="nome:João" (pode ser expandido para JSON ou expressão)
                // Aqui, só um placeholder, pois filtro dinâmico real exige parser
            }

            // Ordenação
            if (!string.IsNullOrEmpty(queryParams.Sort))
            {
                // Exemplo: sort="nome,-dataCadastro"
                // Aqui, só um placeholder, pois ordenação dinâmica real exige parser/reflection
            }

            var totalCount = await query.CountAsync();

            // Paginação
            int skip = queryParams.Start > 0 ? queryParams.Start : (queryParams.Page - 1) * queryParams.Limit;
            var items = await query.Skip(skip).Take(queryParams.Limit).ToListAsync();
            return (items, totalCount);
        }

        public virtual async Task Criar(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.DataCriacao = DateTime.UtcNow;
                baseEntity.DataAtualizacao = null;
            }
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Atualizar(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.DataAtualizacao = DateTime.UtcNow;
            }
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remover(Guid id)
        {
            var entity = await ObterPorId(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
} 