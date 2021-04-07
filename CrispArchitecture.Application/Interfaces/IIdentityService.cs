using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Users;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponseDto> Login(string email, string password);
        Task<AuthResponseDto> Register(string displayName, string email, string password);
    }
}