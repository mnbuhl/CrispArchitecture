using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(Guid id);
        Task<IList<Product>> GetAllProductsAsync();
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Guid id);
    }
}