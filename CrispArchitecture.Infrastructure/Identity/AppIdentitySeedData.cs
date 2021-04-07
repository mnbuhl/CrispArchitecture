using System.Linq;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CrispArchitecture.Infrastructure.Identity
{
    public class AppIdentitySeedData
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManger)
        {
            if (!userManger.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "John",
                    Email = "john@test.com",
                    UserName = "john@test.com",
                    Address = new Address
                    {
                        FirstName = "John",
                        LastName = "Test",
                        Street = "The Street 10",
                        City = "New York",
                        PostalCode = "80000"
                    }
                };

                await userManger.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}