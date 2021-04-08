using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Extensions;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            var user = await _userManager.FindByClaimsPrinciple(User);

            if (user == null)
                return BadRequest(AuthErrorDto.Error(new[] { "User not found" }));

            var authResponse = new AuthResponseDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
            
            return Ok(authResponse);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindByClaimsPrincipleWithAddressAsync(User);

            if (user.Address == null)
                return BadRequest("No Address related to user");

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommandDto loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
                return Unauthorized(AuthErrorDto.Error(new[] { "Bad email and password combination" }));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (!result.Succeeded)
                return Unauthorized(AuthErrorDto.Error(new[] { "Bad email and password combination" }));

            var authResponse = new AuthResponseDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommandDto registerRequest)
        {
            var user = new AppUser
            {
                DisplayName = registerRequest.DisplayName,
                Email = registerRequest.Email,
                UserName = registerRequest.Email
            };
            
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                return BadRequest(AuthErrorDto.Error(result.Errors.Select(x => x.Description)));
            
            var authResponse = new AuthResponseDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

            return Ok(authResponse);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<IActionResult> UpdateUserAddress([FromBody] AddressDto addressRequest)
        {
            var user = await _userManager.FindByClaimsPrincipleWithAddressAsync(User);

            user.Address ??= new Address();

            _mapper.Map(addressRequest, user.Address);

            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
                return BadRequest();

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
    }
}