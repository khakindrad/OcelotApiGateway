using Auth.Api.Interfaces;
using Auth.Api.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Common.ActionFilters;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        [ValidateModel]
        public async Task<ActionResult<AuthResponseDto>> LoginAync([FromBody] LoginDto loginDto)
        {
            var authResponse = await _authService.LoginAsync(loginDto);

            if (authResponse is not null)
            {
                var authResponseDto = _mapper.Map<AuthResponseDto>(authResponse);

                return authResponseDto;
            }

            return BadRequest();
        }

        [HttpPost("Register")]
        [ValidateModel]
        public async Task<ActionResult<RegisterResponseDto>> RegisterAync([FromBody] RegisterDto registerDto)
        {
            var registerResponse = await _authService.RegisterAsync(registerDto);

            if (registerResponse is not null)
            {
                return CreatedAtAction("LoginAync", registerDto);
            }

            return BadRequest();
        }
    }
}
