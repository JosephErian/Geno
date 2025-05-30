using Geno.Models;

namespace Geno.Models
{
    public class Order
    {
        public int I3D { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int OrderByCustomerPhone { get; set; }
        public OrderStatus OrderStatus;
        public DeliveryStatus DeliveryStatus;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public List<Product> Products { get; set; } = new();  // Assuming many-to-many
    };

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
    public enum DeliveryStatus
    {
        Warehouse,
        StockToPostOffice,
        PostOffice,
        OutForDelivery,
        Delivered,
    };
}