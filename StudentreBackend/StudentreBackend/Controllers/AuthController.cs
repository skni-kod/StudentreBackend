using Microsoft.AspNetCore.Mvc;
using StudentreBackend.DTO;
using StudentreBackend.Services;
using StudentreBackend.Services.Interfaces;

namespace StudentreBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] AuthenticationResponse authenticationResponse)
        {
            var token = AuthService.Refresh(authenticationResponse);

            if(String.IsNullOrEmpty(token.JwtToken) || String.IsNullOrEmpty(token.RefreshToken))
            {
                return BadRequest();
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            AuthService.Register(registerDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {
            var token = AuthService.Login(model);

            if(String.IsNullOrEmpty(token.JwtToken) || String.IsNullOrEmpty(token.RefreshToken))
            {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}