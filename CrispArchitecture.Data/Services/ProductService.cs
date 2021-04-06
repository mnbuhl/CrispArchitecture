using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);
        }

        public async Task<IList<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await _unitOfWork.ProductRepository.CreateAsync(product);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _unitOfWork.ProductRepository.Update(product);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            await _unitOfWork.ProductRepository.DeleteAsync(id);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}