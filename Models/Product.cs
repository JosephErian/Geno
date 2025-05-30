using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Geno.Models
{
    public class Product
    {
        public int I3D { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Category Category { get; set; }  // this references the enum type
        public string? Zone { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime StockedAt { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        [NotMapped]
        public Dictionary<string, string>? Details { get; set; }
    }

    public enum Category
    {
        Electronics,
        Accessories,
        Tools
    }
}