using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Interfaces;

namespace BSC.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;

        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestDto requestDto,
            [FromQuery] string? authType)
        {
            var response = await _authApplication.Login(requestDto, authType);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credenciales,
            [FromQuery] string authType)
        {
            var response = await _authApplication.LoginWithGoogle(credenciales, authType);
            return Ok(response);
        }
    }
}