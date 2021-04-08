using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Customers
{
    public class CustomersSpecification : BaseSpecification<Customer>
    {
        public CustomersSpecification(string sort)
        {
            switch (sort)
            {
                case "id":
                    AddOrderBy(x => x.Id);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
    }
}