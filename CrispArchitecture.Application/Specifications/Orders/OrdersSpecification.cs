using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Orders
{
    public class OrdersSpecification : BaseSpecification<Order>
    {
        public OrdersSpecification(string sort)
        {
            switch (sort)
            {
                case "totalAsc":
                    AddOrderBy(x => x.Total);
                    break;
                case "totalDesc":
                    AddOrderByDescending(x => x.Total);
                    break;
                case "dateAsc":
                    AddOrderBy(x => x.CreatedAt);
                    break;
                default:
                    AddOrderByDescending(x => x.CreatedAt);
                    break;
            }
        }
    }
}