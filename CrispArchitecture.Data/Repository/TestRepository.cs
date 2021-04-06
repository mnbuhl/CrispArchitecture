using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Data.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;

        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Test> GetAsync(Guid id)
        {
            var test = await _context.Tests
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return test;
        }

        public async Task<List<Test>> GetAllAsync()
        {
            return await _context.Tests.AsNoTracking().ToListAsync();
        }

        public async Task PostAsync(Test test)
        {
            await _context.Tests.AddAsync(test);
        }

        public void UpdateAsync(Test test)
        {
            _context.Tests.Update(test);
        }

        public async Task DeleteAsync(Guid id)
        {
            var test = await _context.Tests.FindAsync(id);

            _context.Tests.Remove(test);
        }
    }
}