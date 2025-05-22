using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Pedido.Request;
using BSC.Application.Dtos.Pedido.Response;
using BSC.Domain.Entities;

namespace BSC.Application.Interfaces
{
    public interface IPedidoApplication
    {
        Task<BaseResponse<IEnumerable<PedidoResponseDto>>> ListPedidos(BaseFiltersRequest filters);
        Task<BaseResponse<PedidoByIdResponseDto>> PedidoById(int pedidoId);
        Task<BaseResponse<Pedido?>> RegisterPedido(PedidoRequestDto requestDto);
        Task<BaseResponse<Pedido?>> EditPedido(int pedidoId, PedidoRequestDto requestDto);
        Task<BaseResponse<bool>> RemovePedido(int pedidoId);
    }
}
