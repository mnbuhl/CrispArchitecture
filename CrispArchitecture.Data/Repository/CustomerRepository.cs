using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return customer;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await GetAsync(id);
            
            if (customer == null)
                return;
            
            _context.Customers.Remove(customer);
        }
    }
}