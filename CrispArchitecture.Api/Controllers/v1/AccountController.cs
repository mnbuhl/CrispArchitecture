using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities.Identity;
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
            var currentUser = await _identityService.GetCurrentUser(User);
            return Ok(currentUser);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var userAddress = await _identityService.GetUserAddress(User);
            return Ok(_mapper.Map<AddressDto>(userAddress));
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

        [Authorize]
        [HttpPut("address")]
        public async Task<IActionResult> UpdateUserAddress([FromBody] AddressDto addressRequest)
        {
            var addressToUpdate = await _identityService.GetUserAddress(User);
            _mapper.Map(addressRequest, addressToUpdate);
            bool success = await _identityService.UpdateUserAddress(User, addressToUpdate);

            if (!success)
                return BadRequest();

            return Ok(_mapper.Map<AddressDto>(addressToUpdate));
        }
    }
}