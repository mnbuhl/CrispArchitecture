using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.LineItems)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.LineItems)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateAsync(Order order)
        {
            if (order.LineItems != null)
                await _context.LineItems.AddRangeAsync(order.LineItems);

            await _context.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await GetAsync(id);

            if (order == null)
                return;

            if (order.LineItems != null)
                _context.LineItems.RemoveRange(order.LineItems);

            _context.Orders.Remove(order);
        }
    }
}