using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task CreateAsync(Product product);
        void Update(Product product);
        Task DeleteAsync(Guid id);
    }
}