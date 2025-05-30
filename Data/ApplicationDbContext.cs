using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Geno.Models;

namespace Geno.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>()
                .HasKey(c => c.I3D);
            builder.Entity<AttendanceRecord>()
                .HasKey(c => c.I3D);
            builder.Entity<Employee>()
                .HasKey(c => c.I3D);
            builder.Entity<Order>()
                .HasKey(c => c.I3D);
            builder.Entity<Product>()
                .HasKey(c => c.I3D);
            // 3. Configure the one-to-many between Customer → Orders
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Configure the one-to-many between Employee → Orders
            builder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}