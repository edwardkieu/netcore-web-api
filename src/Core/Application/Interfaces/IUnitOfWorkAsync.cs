using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        IProductRepositoryAsync ProductRepo { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task BeginTransactionAsync(CancellationToken cancellationToken);

        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);

        DbContext GetContext();
    }
}