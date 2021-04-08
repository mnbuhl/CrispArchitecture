using System.Collections.Generic;

namespace CrispArchitecture.Application.Contracts.v1.Users
{
    public class AuthResponseDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}