namespace Geno.Models.DTOs
{
    public class CreateOrderDto
    {
        public int I3D { get; set; }
        public int CustomerId { get; set; }
        public string OrderByCustomerPhone { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public List<int> ProductIds { get; set; } = new();  // IDs only
    }

}