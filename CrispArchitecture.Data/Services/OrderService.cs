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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            Expression<Func<Order, bool>> predicate = o => o.Id == id;

            // Local function specifying the includes of the order.
            IIncludableQueryable<Order, object> Includes(IQueryable<Order> x) =>
                x.Include(o => o.LineItems).ThenInclude(li => li.Product);

            var order = await _unitOfWork.OrderRepository.GetAsync(predicate, Includes);

            return order;
        }

        public async Task<IList<Order>> GetAllOrdersAsync()
        {
            IList<Order> orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return orders;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            await _unitOfWork.OrderRepository.CreateAsync(order);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _unitOfWork.OrderRepository.Update(order);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            await _unitOfWork.OrderRepository.DeleteAsync(id);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}