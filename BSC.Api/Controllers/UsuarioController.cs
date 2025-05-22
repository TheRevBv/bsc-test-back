using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Interfaces;

namespace BSC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioApplication userApplication) : ControllerBase
    {
        private readonly IUsuarioApplication _userApplication = userApplication;

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UsuarioRequestDto requestDto)
        {
            var response = await _userApplication.RegisterUser(requestDto);
            return Ok(response);
        }
    }
}