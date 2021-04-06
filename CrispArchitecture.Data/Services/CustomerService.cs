using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CrispArchitecture.Data.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            Expression<Func<Customer, bool>> predicate = c => c.Id == id;

            // Local function specifying the includes of the order.
            IIncludableQueryable<Customer, object> Includes(IQueryable<Customer> x) =>
                x.Include(c => c.Orders);

            var customer = await _unitOfWork.CustomerRepository.GetAsync(predicate, Includes);

            return customer;
        }

        public async Task<IList<Customer>> GetAllCustomerAsync()
        {
            return await _unitOfWork.CustomerRepository.GetAllAsync();
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            await _unitOfWork.CustomerRepository.CreateAsync(customer);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _unitOfWork.CustomerRepository.Update(customer);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            await _unitOfWork.CustomerRepository.DeleteAsync(id);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}