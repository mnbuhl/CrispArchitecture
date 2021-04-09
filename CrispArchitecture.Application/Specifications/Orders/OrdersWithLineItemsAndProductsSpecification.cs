using System;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Specifications.Orders
{
    public class OrdersWithLineItemsAndProductsSpecification : BaseSpecification<Order>
    {
        public OrdersWithLineItemsAndProductsSpecification(BaseSpecificationParams parameters)
        {
            ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
            
            switch (parameters.Sort)
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
            
            AddIncludes(o => o.Include(x => x.LineItems)
                .ThenInclude(li => li.Product));
        }
        
        public OrdersWithLineItemsAndProductsSpecification(Guid id) : base(x => x.Id == id)
        {
            AddIncludes(o => o.Include(x => x.LineItems)
                .ThenInclude(li => li.Product));
        }
    }
}