using System;
using CrispArchitecture.Application.Contracts.v1.Products;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class LineItemResponseDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public ProductResponseDto Product { get; set; }
    }
}