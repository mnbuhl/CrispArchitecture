using Microsoft.AspNetCore.Identity;

namespace CrispArchitecture.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}