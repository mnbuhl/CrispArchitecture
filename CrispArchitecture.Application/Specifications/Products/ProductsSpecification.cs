using System;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Specifications.Products
{
    public class ProductsSpecification : BaseSpecification<Product>
    {
        public ProductsSpecification()
        {
        }

        public ProductsSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}