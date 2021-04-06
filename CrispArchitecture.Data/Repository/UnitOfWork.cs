using System;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<Customer> _customerRepository;
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<Order> _orderRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Customer> CustomerRepository
        {
            get { return _customerRepository ??= new GenericRepository<Customer>(_context); }
        }

        public IGenericRepository<Product> ProductRepository
        {
            get { return _productRepository ??= new GenericRepository<Product>(_context); }
        }
        
        public IGenericRepository<Order> OrderRepository
        {
            get { return _orderRepository ??= new GenericRepository<Order>(_context); }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}