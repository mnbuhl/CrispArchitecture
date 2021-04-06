using System;
using System.Collections.Generic;
using CrispArchitecture.Application.Contracts.v1.Orders;

namespace CrispArchitecture.Application.Contracts.v1.Customers
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<OrderResponseDto> Orders { get; set; }
    }
}