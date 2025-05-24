using Microsoft.AspNetCore.Mvc;
using BSC.Application.Interfaces;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.RolUsuario.Request;
using Microsoft.AspNetCore.Authorization;

namespace BSC.Api.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController(IUsuarioApplication usuarioApp) : ControllerBase
    {
        private readonly IUsuarioApplication _usuarioApp = usuarioApp;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List([FromQuery] BaseFiltersRequest filters) => Ok(await _usuarioApp.ListUsuarios(filters));

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id) => Ok(await _usuarioApp.UsuarioById(id));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UsuarioRequestDto dto) => Ok(await _usuarioApp.RegisterUsuario(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UsuarioRequestDto dto) => Ok(await _usuarioApp.EditUsuario(id, dto));

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id) => Ok(await _usuarioApp.RemoveUsuario(id));

        [HttpPut("asignar-roles/{usuarioId}")]
        [Authorize]
        public async Task<IActionResult> AsignarRoles(int usuarioId, [FromBody] AsignarRolesUsuarioDto dto) => Ok(await _usuarioApp.AsignarRolesUsuario(usuarioId, dto));
    }
}