using System;

namespace CrispArchitecture.Application.Contracts.v1.Products
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ProductGroup { get; set; }
    }
}