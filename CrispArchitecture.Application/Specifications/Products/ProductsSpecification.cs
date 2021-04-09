using System;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Specifications.Products
{
    public class ProductsSpecification : BaseSpecification<Product>
    {
        public ProductsSpecification(ProductParams parameters, bool count = false)
            : base(x => 
                (string.IsNullOrEmpty(parameters.Search) || x.Name.ToLower().Contains(parameters.Search)) &&
                (!parameters.ProductGroupId.HasValue || x.ProductGroupId == parameters.ProductGroupId))
        {
            if (count)
                return;
            
            ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
            
            switch (parameters.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                case "nameDesc":
                    AddOrderByDescending(x => x.Name);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
            
            AddIncludes(x => x.Include(p => p.ProductGroup));
        }

        public ProductsSpecification(Guid id) : base(x => x.Id == id)
        {
            AddIncludes(x => x.Include(p => p.ProductGroup));
        }
    }
}