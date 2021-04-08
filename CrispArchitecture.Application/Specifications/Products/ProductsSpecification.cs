using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Products
{
    public class ProductsSpecification : BaseSpecification<Product>
    {
        public ProductsSpecification(string sort)
        {
            switch (sort)
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
        }

        public ProductsSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}