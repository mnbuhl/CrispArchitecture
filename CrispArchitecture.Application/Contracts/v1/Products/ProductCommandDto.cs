using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Products
{
    public class ProductCommandDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
    }
}