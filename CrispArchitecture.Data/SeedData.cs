using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Data
{
    public class SeedData
    {
        public static async Task SeedDatabase(AppDbContext context)
        {
            if (!context.Tests.Any())
            {
                List<Test> tests = new List<Test>
                {
                    new Test
                    {
                        TestValue1 = "test1",
                        TestValue2 = 0,
                        TestEmail = "helloworld@example.com"
                    },
                    new Test
                    {
                        TestValue1 = "test2",
                        TestValue2 = 1,
                        TestEmail = "helloworld1@example.com"
                    },
                    new Test
                    {
                        TestValue1 = "test3",
                        TestValue2 = 2,
                        TestEmail = "helloworld4@example.com"
                    },
                    new Test
                    {
                        TestValue1 = "test4",
                        TestValue2 = 3,
                        TestEmail = "helloworld2@example.com"
                    },
                    new Test
                    {
                        TestValue1 = "test5",
                        TestValue2 = 4,
                        TestEmail = "helloworld6@example.com"
                    }
                };
                await context.Tests.AddRangeAsync(tests);
            }

            if (!context.TestOwners.Any())
            {
                List<TestOwner> testOwners = new List<TestOwner>
                {
                    new TestOwner
                    {
                        TestId = Guid.Parse("C7D769C5-87E3-4B81-5E1E-08D8F8A3E23D")
                    },
                    new TestOwner
                    {
                        TestId = Guid.Parse("3A518A69-6CE6-4761-5E1F-08D8F8A3E23D")
                    },
                    new TestOwner
                    {
                        TestId = Guid.Parse("E924A9E6-54F3-4C2E-5E20-08D8F8A3E23D")
                    }
                };

                await context.TestOwners.AddRangeAsync(testOwners);
            }
            
            await context.SaveChangesAsync();
        }
    }
}