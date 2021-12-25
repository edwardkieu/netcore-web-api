using Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T>, IDisposable where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public async Task<T> FindByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            var entity = _context.Set<T>().Find(id);
            return await FindAll(includeProperties).FirstOrDefaultAsync(x => x.Equals(entity));
        }

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity, bool isSaveChange = false)
        {
            await _context.AddAsync(entity);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, bool isSaveChange = false)
        {
            await _context.AddRangeAsync(entities);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T entity, bool isSaveChange = false)
        {
            _context.Set<T>().Update(entity);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, bool isSaveChange = false)
        {
            _context.Set<T>().UpdateRange(entities);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(T entity, bool isSaveChange = false)
        {
            _context.Set<T>().Remove(entity);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(object id, bool isSaveChange = false)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                await RemoveAsync(entity);
                if (isSaveChange)
                {
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities, bool isSaveChange = false)
        {
            _context.Set<T>().RemoveRange(entities);
            if (isSaveChange)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRangeAsync(Expression<Func<T, bool>> predicate, bool isSaveChange = false)
        {
            var entities = await _context.Set<T>().Where(predicate).ToListAsync();
            if (entities.Any())
            {
                _context.Set<T>().RemoveRange(entities);
            }
            if (isSaveChange)
                await _context.SaveChangesAsync();
        }

        #region Dispose

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}