using System.Security.Claims;
using System.Threading.Tasks;
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

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            string email = HttpContext.User?.FindFirstValue(ClaimTypes.Email);

            var currentUser = await _identityService.GetCurrentUser(email);

            return Ok(currentUser);
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