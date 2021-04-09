using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Customers
{
    public class CustomersSpecification : BaseSpecification<Customer>
    {
        public CustomersSpecification(BaseSpecificationParams parameters)
        {
            ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
            
            switch (parameters.Sort)
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