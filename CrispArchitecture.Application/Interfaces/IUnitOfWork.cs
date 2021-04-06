using System;
using System.Threading.Tasks;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Add future repositories to this interface
        ITestRepository TestRepository { get; }
        ITestOwnerRepository TestOwnerRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<int> SaveAsync();
    }
}