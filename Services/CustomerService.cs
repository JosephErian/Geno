using System.Collections.Generic;
using System.Threading.Tasks;
using Geno.Data;
using Geno.Models;
using Geno.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Geno.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync() => await _db.Customers.ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id) => await _db.Customers.FindAsync(id);

        public async Task<List<Customer>> FilterAsync(string? name, int? id)
        {
            return await _db.Customers
                            .Where(c => string.IsNullOrEmpty(name) || c.FirstName.Contains(name) || c.I3D == id)
                            .ToListAsync();
        }

        public async Task<Customer?> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                Email = dto.Email,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Address = dto.Address,
                PostalCode = "",
                Orders = new List<Order>(),
                TotalQuantity = 0
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return customer;
        }
        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var existing = await _db.Customers.FirstOrDefaultAsync(p => p.I3D == customer.I3D);

            if (existing == null)
                return null; // or throw exception

            // Update properties
            existing.Email = customer.Email;

            await _db.SaveChangesAsync();

            return existing;
        }
    }
}