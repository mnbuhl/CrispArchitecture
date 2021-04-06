using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetAsync(Guid id);
        Task<List<Order>> GetAllAsync();
        Task CreateAsync(Order order);
        void Update(Order order);
        Task DeleteAsync(Guid id);
    }
}