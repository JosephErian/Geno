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

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(p => p.I3D == product.I3D);

            if (existing == null)
                return null; // or throw exception

            // Update properties
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;
            existing.Zone = product.Zone;
            existing.ExpiredAt = product.ExpiredAt;
            existing.StockedAt = product.StockedAt;
            existing.Quantity = product.Quantity;
            existing.Rate = product.Rate;
            existing.Details = product.Details;

            await _db.SaveChangesAsync();

            return existing;
        }
    }
}