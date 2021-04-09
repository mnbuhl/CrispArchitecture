using System;
using System.Collections.Generic;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public double Total { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<LineItemResponseDto> LineItems { get; set; }
    }
}