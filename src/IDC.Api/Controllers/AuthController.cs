using IDC.Application.Dto.Auth;
using IDC.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IDC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Invalid request");

            var result = await _authService.Login(request);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticatedResult>> RefreshToken([FromBody] TokenRequest request)
        {
            if (request == null) return BadRequest("Invalid request");
            var result = await _authService.RefreshToken(request);
            return Ok(result);
        }
    }
}
