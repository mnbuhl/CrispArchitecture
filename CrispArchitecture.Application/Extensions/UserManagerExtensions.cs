using System.Security.Claims;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Application.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByClaimsPrincipleWithAddressAsync(this UserManager<AppUser> userManager,
            ClaimsPrincipal userClaims)
        {
            string email = userClaims.FindFirstValue(ClaimTypes.Email);
            return await userManager.Users
                .Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByClaimsPrinciple(this UserManager<AppUser> userManager,
            ClaimsPrincipal userClaims)
        {
            string email = userClaims.FindFirstValue(ClaimTypes.Email);
            return await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}