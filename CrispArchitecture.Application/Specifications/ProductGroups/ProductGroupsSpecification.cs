using System;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Specifications.ProductGroups
{
    public class ProductGroupsSpecification : BaseSpecification<ProductGroup>
    {
        public ProductGroupsSpecification(BaseSpecificationParams parameters, bool count = false)
        {
            if (count)
                return;
            
            ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
            
            switch (parameters.Sort)
            {
                case "nameDesc":
                    AddOrderByDescending(x => x.Name);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
            
            AddIncludes(x => x.Include(p => p.Products));
        }

        public ProductGroupsSpecification(Guid id) : base(x => x.Id == id)
        {
            AddIncludes(x => x.Include(p => p.Products));
        }
    }
}