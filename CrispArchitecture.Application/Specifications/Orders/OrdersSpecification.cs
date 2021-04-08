using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Orders
{
    public class OrdersSpecification : BaseSpecification<Order>
    {
        public OrdersSpecification(SpecificationParams parameters)
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
        }
    }
}