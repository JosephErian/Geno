using System.Collections.Generic;
using System.Threading.Tasks;
using Geno.Data;
using Geno.Models;
using Microsoft.EntityFrameworkCore;

namespace Geno.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetAllAsync() => await _db.Orders.ToListAsync();

        public async Task<Order?> GetByIdAsync(int id) => await _db.Orders.FindAsync(id);

        public async Task<List<Order>> FilterAsync(string? name)
        {
            return await _db.Orders
                            .Where(o => string.IsNullOrEmpty(name))
                            .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            var existing = await _db.Orders.FirstOrDefaultAsync(p => p.I3D == order.I3D);

            if (existing == null)
                return null; // or throw exception

            // Update properties
            existing.TotalPrice = order.TotalPrice;
            existing.TotalQuantity = order.TotalQuantity;
            existing.DeliveryStatus = order.DeliveryStatus;
            existing.OrderStatus = order.OrderStatus;

            await _db.SaveChangesAsync();

            return existing;
        }
    }
}