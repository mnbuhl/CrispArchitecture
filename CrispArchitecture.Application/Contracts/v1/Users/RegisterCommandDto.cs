using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Users
{
    public class RegisterCommandDto
    {
        [RegularExpression("^[a-zA-Z0-9æøå ]*$")]
        public string DisplayName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}