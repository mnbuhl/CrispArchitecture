using System;
using System.Collections.Generic;
using CrispArchitecture.Application.Contracts.v1.Products;

namespace CrispArchitecture.Application.Contracts.v1.ProductGroups
{
    public class ProductGroupWithProductsResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductResponseDto> Products { get; set; }
    }
}