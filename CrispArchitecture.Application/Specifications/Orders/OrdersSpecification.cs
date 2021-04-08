using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Orders
{
    public class OrdersSpecification : BaseSpecification<Order>
    {
        public OrdersSpecification()
        {
        }

        public OrdersSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}