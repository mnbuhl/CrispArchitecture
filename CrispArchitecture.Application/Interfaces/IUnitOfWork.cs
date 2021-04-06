using System;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Add future repositories to this interface
        IGenericRepository<Customer> CustomerRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        Task<int> SaveAsync();
    }
}