using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Application.Extensions;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CrispArchitecture.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
        }

        public async Task<AuthResponseDto> GetCurrentUser(ClaimsPrincipal appUser)
        {
            var user = await _userManager.FindByClaimsPrinciple(appUser);

            if (user == null)
                return new AuthResponseDto { Errors = new[] { "User not found" } };

            return new AuthResponseDto
            {
                Email = user.Email,
                Token = CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        public async Task<Address> GetUserAddress(ClaimsPrincipal appUser)
        {
            var user = await _userManager.FindByClaimsPrincipleWithAddressAsync(appUser);

            return user.Address;
        }

        public async Task<AuthResponseDto> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthResponseDto { Errors = new[] { "Bad email and/or password combination" } };

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return new AuthResponseDto { Errors = new[] { "Bad email and/or password combination" } };

            return new AuthResponseDto
            {
                Email = user.Email,
                Token = CreateToken(user),
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
                Token = CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        public async Task<bool> UpdateUserAddress(ClaimsPrincipal appUser, Address addressToUpdate)
        {
            var user = await _userManager.FindByClaimsPrincipleWithAddressAsync(appUser);

            user.Address = addressToUpdate;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        private string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Issuer = _configuration["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}