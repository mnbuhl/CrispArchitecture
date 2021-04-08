using CrispArchitecture.Domain.Entities.Identity;

namespace CrispArchitecture.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}