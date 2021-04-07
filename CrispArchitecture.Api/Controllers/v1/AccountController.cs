using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AccountController(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            var currentUser = await _identityService.GetCurrentUser(HttpContext.User);
            return Ok(currentUser);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var userAddress = await _identityService.GetUserAddress(HttpContext.User);
            return Ok(_mapper.Map<AddressResponseDto>(userAddress));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommandDto loginRequest)
        {
            var authResponse = await _identityService.Login(loginRequest.Email, loginRequest.Password);

            if (authResponse.Errors != null)
                return Unauthorized(authResponse.Errors);

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommandDto registerRequest)
        {
            var authResponse = await _identityService.Register(registerRequest.DisplayName, registerRequest.Email,
                registerRequest.Password);

            if (authResponse.Errors != null)
                return BadRequest(authResponse.Errors);

            return Ok(authResponse);
        }
    }
}