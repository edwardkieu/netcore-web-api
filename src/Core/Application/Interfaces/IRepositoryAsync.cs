using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T> FindByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity, bool isSaveChange = true);

        Task AddRangeAsync(IEnumerable<T> entities, bool isSaveChange = false);

        Task UpdateAsync(T entity, bool isSaveChange = false);

        Task UpdateRangeAsync(IEnumerable<T> entities, bool isSaveChange = false);

        Task RemoveAsync(T entity, bool isSaveChange = false);

        Task RemoveAsync(object id, bool isSaveChange = false);

        Task RemoveRangeAsync(IEnumerable<T> entities, bool isSaveChange = false);

        Task RemoveRangeAsync(Expression<Func<T, bool>> predicate, bool isSaveChange = false);
    }
}