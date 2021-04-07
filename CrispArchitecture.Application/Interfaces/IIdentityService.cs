using System.Security.Claims;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Domain.Entities.Identity;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponseDto> GetCurrentUser(ClaimsPrincipal appUser);
        Task<Address> GetUserAddress(ClaimsPrincipal appUser);
        Task<AuthResponseDto> Login(string email, string password);
        Task<AuthResponseDto> Register(string displayName, string email, string password);
        Task<bool> UpdateUserAddress(ClaimsPrincipal appUser, Address addressToUpdate);
    }
}