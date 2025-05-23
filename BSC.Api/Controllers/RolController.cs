using Microsoft.AspNetCore.Mvc;
using BSC.Application.Interfaces;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Dtos.Rol.Request;
using BSC.Utilities.Static;
using Microsoft.AspNetCore.Authorization;

namespace BSC.Api.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolController(IRolApplication rolApplication) : ControllerBase
    {
        private readonly IRolApplication _rolApplication = rolApplication;

        [HttpGet]
        public async Task<IActionResult> ListRoles([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _rolApplication.ListRoles(filters);

            if ((bool)filters.Download!)
            {
                // var columnNames = ExcelColumnNames.GetColumnsRoles();
                //var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                //return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("select")]
        public async Task<IActionResult> ListSelectRoles()
        {
            var response = await _rolApplication.ListSelectRoles();
            return Ok(response);
        }

        [HttpGet("{rolId:int}")]
        public async Task<IActionResult> RolById(int rolId)
        {
            var response = await _rolApplication.RolById(rolId);
            return Ok(response);
        }

        [HttpPost]
        // [Authorize(Roles = nameof(RolesTypes.Administrador))]
        public async Task<IActionResult> RegisterRol([FromBody] RolRequestDto requestDto)
        {
            var response = await _rolApplication.RegisterRol(requestDto);
            return Ok(response);
        }

        [HttpPut("{rolId:int}")]
        // [Authorize(Roles = nameof(RolesTypes.Administrador))]
        public async Task<IActionResult> EditRol(int rolId, [FromBody] RolRequestDto requestDto)
        {
            var response = await _rolApplication.EditRol(rolId, requestDto);
            return Ok(response);
        }

        [HttpDelete("{rolId:int}")]
        // [Authorize(Roles = nameof(RolesTypes.Administrador))]
        public async Task<IActionResult> RemoveRol(int rolId)
        {
            var response = await _rolApplication.RemoveRol(rolId);
            return Ok(response);
        }
    }
}