using System;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Specifications.Customers
{
    public class CustomersWithOrdersSpecification : BaseSpecification<Customer>
    {
        public CustomersWithOrdersSpecification()
        {
            SetIncludes(c => c.Include(x => x.Orders));
        }

        public CustomersWithOrdersSpecification(Guid id) : base(x => x.Id == id)
        {
            SetIncludes(c => c.Include(x => x.Orders));
        }
    }
}