using BSC.Application.Dtos.PedidoProducto.Response;

namespace BSC.Application.Dtos.Pedido.Response
{
    public class PedidoByIdResponseDto
    {
        public int PedidoId { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime FechaPedido { get; set; }
        public int UsuarioId { get; set; }
        public List<PedidoProductoResponseDto> Productos { get; set; } = [];
    }
}
