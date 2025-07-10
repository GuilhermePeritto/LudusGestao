using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using LudusGestao.Infrastructure.Data.Context;
using LudusGestao.Domain.Common;
using System.Linq;
using System.Text.Json;
using System.Linq.Expressions;

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

            // Aplicar filtros
            if (!string.IsNullOrEmpty(queryParams.Filter))
            {
                query = ApplyFilters(query, queryParams.Filter);
            }

            // Aplicar ordenação
            if (!string.IsNullOrEmpty(queryParams.Sort))
            {
                query = ApplySorting(query, queryParams.Sort);
            }

            var totalCount = await query.CountAsync();

            // Paginação
            int skip = queryParams.Start > 0 ? queryParams.Start : (queryParams.Page - 1) * queryParams.Limit;
            var items = await query.Skip(skip).Take(queryParams.Limit).ToListAsync();
            return (items, totalCount);
        }

        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query, string filter)
        {
            try
            {
                // Tentar parsear como JSON primeiro
                if (filter.StartsWith("{") && filter.EndsWith("}"))
                {
                    return ApplyJsonFilter(query, filter);
                }

                // Filtro simples no formato "campo:valor"
                var parts = filter.Split(':', 2);
                if (parts.Length == 2)
                {
                    var field = parts[0].Trim();
                    var value = parts[1].Trim();
                    return ApplySimpleFilter(query, field, value);
                }

                // Busca por texto em todos os campos de string
                return ApplyTextSearch(query, filter);
            }
            catch
            {
                // Se houver erro no parsing, retorna a query original
                return query;
            }
        }

        protected virtual IQueryable<T> ApplyJsonFilter(IQueryable<T> query, string jsonFilter)
        {
            try
            {
                var filters = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonFilter);
                foreach (var filter in filters)
                {
                    query = ApplySimpleFilter(query, filter.Key, filter.Value?.ToString());
                }
                return query;
            }
            catch
            {
                return query;
            }
        }

        protected virtual IQueryable<T> ApplySimpleFilter(IQueryable<T> query, string field, string value)
        {
            if (string.IsNullOrEmpty(value)) return query;

            // Mapeamento de campos comuns
            var fieldMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "nome", "Nome" },
                { "email", "Email" },
                { "documento", "Documento" },
                { "telefone", "Telefone" },
                { "situacao", "Situacao" },
                { "data", "Data" },
                { "dataCriacao", "DataCriacao" },
                { "valor", "Valor" },
                { "esporte", "Esporte" },
                { "cor", "Cor" }
            };

            var actualField = fieldMappings.ContainsKey(field) ? fieldMappings[field] : field;

            // Aplicar filtro usando reflection
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var property = System.Linq.Expressions.Expression.Property(parameter, actualField);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            
            if (containsMethod != null)
            {
                var containsCall = System.Linq.Expressions.Expression.Call(property, containsMethod, constant);
                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(containsCall, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        protected virtual IQueryable<T> ApplyTextSearch(IQueryable<T> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            // Busca em campos de texto comuns
            var textFields = new[] { "Nome", "Email", "Documento", "Telefone", "Observacoes", "Esporte" };
            
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var searchConstant = System.Linq.Expressions.Expression.Constant(searchTerm.ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            var conditions = new List<System.Linq.Expressions.Expression>();

            foreach (var field in textFields)
            {
                try
                {
                    var property = System.Linq.Expressions.Expression.Property(parameter, field);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var toLowerCall = System.Linq.Expressions.Expression.Call(property, toLowerMethod);
                    var containsCall = System.Linq.Expressions.Expression.Call(toLowerCall, containsMethod, searchConstant);
                    conditions.Add(containsCall);
                }
                catch
                {
                    // Campo não existe na entidade, ignorar
                }
            }

            if (conditions.Any())
            {
                var combinedCondition = conditions.Aggregate((a, b) => 
                    System.Linq.Expressions.Expression.Or(a, b));
                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        protected virtual IQueryable<T> ApplySorting(IQueryable<T> query, string sort)
        {
            if (string.IsNullOrEmpty(sort)) return query;

            var sortFields = sort.Split(',');
            var isFirst = true;

            foreach (var sortField in sortFields)
            {
                var trimmedField = sortField.Trim();
                var isDescending = trimmedField.StartsWith("-");
                var actualField = isDescending ? trimmedField.Substring(1) : trimmedField;

                // Mapeamento de campos
                var fieldMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "nome", "Nome" },
                    { "email", "Email" },
                    { "documento", "Documento" },
                    { "data", "Data" },
                    { "dataCriacao", "DataCriacao" },
                    { "valor", "Valor" },
                    { "situacao", "Situacao" }
                };

                var mappedField = fieldMappings.ContainsKey(actualField) ? fieldMappings[actualField] : actualField;

                try
                {
                    if (isFirst)
                    {
                        query = isDescending ? query.OrderByDescending(e => EF.Property<object>(e, mappedField)) 
                                            : query.OrderBy(e => EF.Property<object>(e, mappedField));
                        isFirst = false;
                    }
                    else
                    {
                        // Para ordenação múltipla, usar OrderBy novamente
                        query = isDescending ? query.OrderByDescending(e => EF.Property<object>(e, mappedField)) 
                                            : query.OrderBy(e => EF.Property<object>(e, mappedField));
                    }
                }
                catch
                {
                    // Campo não existe, ignorar
                }
            }

            return query;
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