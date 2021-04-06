using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Data.Repository
{
    public class TestOwnerRepository : ITestOwnerRepository
    {
        private readonly AppDbContext _context;

        public TestOwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TestOwner> GetAsync(Guid id)
        {
            var testOwner = await _context.TestOwners
                .Include(t => t.Test)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return testOwner;
        }

        public async Task<List<TestOwner>> GetAllAsync()
        {
            return await _context.TestOwners
                .Include(t => t.Test)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task PostAsync(TestOwner testOwner)
        {
            await _context.TestOwners.AddAsync(testOwner);
        }

        public void Update(TestOwner testOwner)
        {
            _context.TestOwners.Update(testOwner);
        }

        public async Task DeleteAsync(Guid id)
        {
            var test = await _context.TestOwners.FindAsync(id);

            _context.TestOwners.Remove(test);
        }
    }
}