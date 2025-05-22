namespace BSC.Application.Dtos.Pedido.Response
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime FechaPedido { get; set; }
    }
}
