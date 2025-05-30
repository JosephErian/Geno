namespace Geno.Models
{
    public class Employee
    {
        public int I3D { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public enum Department;
        public string Email { get; set; }
        // public List<Order> OrdersPlaced { get; set; }
        public List<Order> Orders { get; set; } = new();
    };

    public enum Department
    {
        Sales,
        Inventory,
        Purchase,
        HR
    }
}