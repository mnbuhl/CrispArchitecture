using System;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;

namespace CrispArchitecture.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ITestRepository _testRepository;
        private ITestOwnerRepository _testOwnerRepository;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITestRepository TestRepository
        {
            get { return _testRepository ??= new TestRepository(_context); }
        }
        
        public ITestOwnerRepository TestOwnerRepository
        {
            get { return _testOwnerRepository ??= new TestOwnerRepository(_context); }
        }

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository ??= new CustomerRepository(_context); }
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository ??= new ProductRepository(_context); }
        }
        
        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
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