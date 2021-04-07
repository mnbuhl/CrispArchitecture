using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CrispArchitecture.Infrastructure.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            Expression<Func<Order, bool>> predicate = o => o.Id == id;

            // Local function specifying the includes of the order.
            IIncludableQueryable<Order, object> Includes(IQueryable<Order> x) =>
                x.Include(o => o.LineItems).ThenInclude(li => li.Product);

            var order = await _orderRepository.GetAsync(predicate, Includes);

            return order;
        }

        public async Task<IList<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            order.Total = await GetOrderTotal(order.LineItems);
            await _orderRepository.CreateAsync(order);
            return await _orderRepository.SaveAsync();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            order.Total += await GetOrderTotal(order.LineItems);
            _orderRepository.Update(order);
            return await _orderRepository.SaveAsync();
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
            return await _orderRepository.SaveAsync();
        }
        
        private async Task<double> GetOrderTotal(ICollection<LineItem> lineItems)
        {
            double total = 0;

            foreach (var lineItem in lineItems)
            {
                var product = await _productRepository.GetAsync(x => x.Id == lineItem.ProductId);

                total += product.Price * lineItem.Amount;
            }

            return total;
        }
    }
}