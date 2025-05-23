namespace BSC.Application.Dtos.Pedido.Response;

public class PedidoResponseDto
{
    public int PedidoId { get; set; }
    public string Cliente { get; set; } = null!;
    public DateTime FechaPedido { get; set; }
    public int UsuarioId { get; set; }
    public int TotalProductos => Productos?.Count ?? 0;

    public List<PedidoProductoSimpleDto> Productos { get; set; } = [];
}

public class PedidoProductoSimpleDto
{
    public int ProductoId { get; set; }
    public string Clave { get; set; } = null!;
    public int Cantidad { get; set; }
    public string ProductoNombre { get; set; } = null!;
    public decimal? Precio { get; set; }
}