using System.Linq;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CrispArchitecture.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<AuthResponseDto> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthResponseDto{ Errors = new []{ "Bad email and/or password combination" } };

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return new AuthResponseDto{ Errors = new []{ "Bad email and/or password combination" } };

            return new AuthResponseDto
            {
                Email = user.Email,
                Token = "This will be a JWT token",
                DisplayName = user.DisplayName
            };
        }

        public async Task<AuthResponseDto> Register(string displayName, string email, string password)
        {
            var user = new AppUser
            {
                DisplayName = displayName,
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return new AuthResponseDto { Errors = result.Errors.Select(x => x.Description) };
            
            return new AuthResponseDto
            {
                Email = user.Email,
                Token = "This will be a JWT token",
                DisplayName = user.DisplayName
            };
        }

        private string CreateToken(AppUser user)
        {
            throw new System.NotImplementedException();
        }
    }
}