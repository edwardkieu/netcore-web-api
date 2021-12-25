using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : RepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly ApplicationDbContext _context;

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _context.Products.AllAsync(p => p.Barcode != barcode);
        }
    }
}