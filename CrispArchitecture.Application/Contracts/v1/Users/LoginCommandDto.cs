using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Users
{
    public class LoginCommandDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}