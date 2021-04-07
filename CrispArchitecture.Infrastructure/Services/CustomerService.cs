using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CrispArchitecture.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;

        public CustomerService(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            Expression<Func<Customer, bool>> predicate = c => c.Id == id;

            // Local function specifying the includes of the order.
            IIncludableQueryable<Customer, object> Includes(IQueryable<Customer> x) =>
                x.Include(c => c.Orders);

            var customer = await _customerRepository.GetAsync(predicate, Includes);

            return customer;
        }

        public async Task<IList<Customer>> GetAllCustomerAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            await _customerRepository.CreateAsync(customer);
            return await _customerRepository.SaveAsync();
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _customerRepository.Update(customer);
            return await _customerRepository.SaveAsync();
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
            return await _customerRepository.SaveAsync();
        }
    }
}