using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetAsync(Guid id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
                return;
            
            _context.Products.Remove(product);
        }
    }
}