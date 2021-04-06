using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface ITestOwnerRepository
    {
        Task<TestOwner> GetAsync(Guid id);
        Task<List<TestOwner>> GetAllAsync();
        Task PostAsync(TestOwner testOwner);
        void UpdateAsync(TestOwner test);
        Task DeleteAsync(Guid id);
    }
}