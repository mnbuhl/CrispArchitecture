using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _productRepository.GetAsync(p => p.Id == id);
        }

        public async Task<IList<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await _productRepository.CreateAsync(product);
            return await _productRepository.SaveAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
            return await _productRepository.SaveAsync();
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
            return await _productRepository.SaveAsync();
        }
    }
}