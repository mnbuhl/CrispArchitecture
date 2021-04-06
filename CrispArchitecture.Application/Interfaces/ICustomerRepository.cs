using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetAsync(Guid id);
        Task<List<Customer>> GetAllAsync();
        Task CreateAsync(Customer customer);
        void Update(Customer customer);
        Task DeleteAsync(Guid id);
    }
}