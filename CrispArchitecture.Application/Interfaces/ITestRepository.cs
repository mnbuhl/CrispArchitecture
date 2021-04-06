using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface ITestRepository
    {
        Task<Test> GetAsync(Guid id);
        Task<List<Test>> GetAllAsync();
        Task PostAsync(Test test);
        void Update(Test test);
        Task DeleteAsync(Guid id);
    }
}