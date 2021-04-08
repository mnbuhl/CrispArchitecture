using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Customers
{
    public class CustomersSpecification : BaseSpecification<Customer>
    {
        public CustomersSpecification()
        {
        }

        public CustomersSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}