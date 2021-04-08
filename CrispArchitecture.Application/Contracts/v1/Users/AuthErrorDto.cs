using System.Collections.Generic;

namespace CrispArchitecture.Application.Contracts.v1.Users
{
    public class AuthErrorDto
    {
        public IEnumerable<string> Errors { get; set; }

        public static AuthErrorDto Error(IEnumerable<string> errors)
        {
            return new AuthErrorDto { Errors = errors };
        }
    }
}