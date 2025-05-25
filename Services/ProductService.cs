using System.Collections.Generic;
using System.Threading.Tasks;
using Geno.Data;
using Geno.Models;
using Microsoft.EntityFrameworkCore;

namespace Geno.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetAllAsync() => await _db.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) => await _db.Products.FindAsync(id);

        public async Task<List<Product>> FilterAsync(string? name)
        {
            return await _db.Products
                            .Where(p => string.IsNullOrEmpty(name) || p.Name.Contains(name))
                            .ToListAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}