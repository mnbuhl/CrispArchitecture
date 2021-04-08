using System;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Specifications.Orders
{
    public class OrdersWithLineItemsAndProductsSpecification : BaseSpecification<Order>
    {
        public OrdersWithLineItemsAndProductsSpecification()
        {
            SetIncludes(o => o.Include(x => x.LineItems)
                .ThenInclude(li => li.Product));
        }

        public OrdersWithLineItemsAndProductsSpecification(Guid id) : base(x => x.Id == id)
        {
            SetIncludes(o => o.Include(x => x.LineItems)
                .ThenInclude(li => li.Product));
        }
    }
}