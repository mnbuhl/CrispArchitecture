using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Specifications;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetAsync(ISpecification<T> specification);
        Task<IList<T>> GetAllAsync(ISpecification<T> specification);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}