using System.Collections.Generic;
using System.Threading.Tasks;
using Geno.Data;
using Geno.Models;
using Microsoft.EntityFrameworkCore;

namespace Geno.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _db;

        public EmployeeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Employee>> GetAllAsync() => await _db.Employees.ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id) => await _db.Employees.FindAsync(id);

        public async Task<List<Employee>> FilterAsync(string? name)
        {
            return await _db.Employees
                            .Where(o => string.IsNullOrEmpty(name))
                            .ToListAsync();
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existing = await _db.Employees.FirstOrDefaultAsync(p => p.I3D == employee.I3D);

            if (existing == null)
                return null; // or throw exception

            // Update properties
            existing.Email = employee.Email;
            existing.Address = employee.Address;
            existing.Phone = employee.Phone;
            existing.PostalCode = employee.PostalCode;
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;

            await _db.SaveChangesAsync();

            return existing;
        }
    }
}