// PedidoController.cs
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Pedido.Request;
using BSC.Application.Dtos.Pedido.Response;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSC.Api.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "v1")]
    public class PedidoController(IPedidoApplication pedidoApplication) : ControllerBase
    {
        private readonly IPedidoApplication _pedidoApplication = pedidoApplication;

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PedidoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListPedidos([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _pedidoApplication.ListPedidos(filters);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<PedidoByIdResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            var response = await _pedidoApplication.PedidoById(id);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<Pedido>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterPedido([FromBody] PedidoRequestDto requestDto)
        {
            var response = await _pedidoApplication.RegisterPedido(requestDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<Pedido>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditPedido(int id, [FromBody] PedidoRequestDto requestDto)
        {
            var response = await _pedidoApplication.EditPedido(id, requestDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePedido(int id)
        {
            var response = await _pedidoApplication.RemovePedido(id);
            return Ok(response);
        }
    }
}