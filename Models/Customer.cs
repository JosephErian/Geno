namespace Geno.Models
{
    public class Customer
    {
        public int I3D { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public PaymentMethod PaymentMethod;
        // public List<Order> OrdersPlaced { get; set; }
        public List<Order> Orders { get; set; } = new();
        public int TotalQuantity { get; set; }

        // public List<Product> Products { get; set; }
    };
    public enum PaymentMethod
    {
        Online,
        CashOnDelivery
    }
}