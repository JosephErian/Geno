using System.Collections.Generic;
using System.Threading.Tasks;
using Geno.Data;
using Geno.Models;
using Geno.Models.DTOs;
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
        public async Task<List<Order>> GetByCustomerId(int id)
        {
            return await _db.Orders
                            .Where(o => o.CustomerId == id)
                            .ToListAsync();
        }

        public async Task<Order> CreateOrUpdateAsync(CreateOrderDto dto)
        {
            // Validate related entities
            var customer = await _db.Customers.FindAsync(dto.CustomerId);
            var employee = await _db.Employees.FindAsync(dto.EmployeeId);
            var products = await _db.Products
                .Where(p => dto.ProductIds.Contains(p.I3D))
                .ToListAsync();

            if (customer == null || products.Count != dto.ProductIds.Count)
                return null;

            else
            {
                employee = await _db.Employees.FindAsync(dto.EmployeeId);
            }
            Order? order;

            if (dto.I3D == 0)
            {
                // Create new order
                order = new Order
                {
                    CustomerId = dto.CustomerId,
                    EmployeeId = dto.EmployeeId,
                    OrderByCustomerPhone = dto.OrderByCustomerPhone,
                    OrderStatus = OrderStatus.Pending,
                    DeliveryStatus = DeliveryStatus.Warehouse,
                    TotalQuantity = products.Sum(p => p.Quantity),
                    TotalPrice = products.Sum(p => p.Price * p.Quantity),
                    Products = products
                };

                _db.Orders.Add(order);
            }
            else
            {
                // Update existing order
                order = await _db.Orders
                    .Include(o => o.Products)
                    .FirstOrDefaultAsync(o => o.I3D == dto.I3D);

                if (order == null)
                    return null;

                order.CustomerId = dto.CustomerId;
                order.EmployeeId = dto.EmployeeId ?? -1;
                order.OrderByCustomerPhone = dto.OrderByCustomerPhone;
                order.TotalQuantity = products.Sum(p => p.Quantity);
                order.TotalPrice = products.Sum(p => p.Price * p.Quantity);
                order.Products = products;

                _db.Orders.Update(order);
            }

            await _db.SaveChangesAsync();
            return order;
        }
    }
}