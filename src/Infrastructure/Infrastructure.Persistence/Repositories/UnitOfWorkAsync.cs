using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _objTran;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _objTran = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_objTran != null)
                await _objTran.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_objTran != null)
            {
                await _objTran.RollbackAsync(cancellationToken);
                await _objTran.DisposeAsync();
            }
        }

        public DbContext GetContext()
        {
            if (_context == null)
            {
                var serviceScope = _serviceProvider.CreateScope();
                return serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            }
            return _context;
        }

        #region Repositories
        public IProductRepositoryAsync ProductRepo => new ProductRepositoryAsync(_context);
        #endregion

        #region IDisposable Support

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

        #endregion IDisposable Support
    }
}