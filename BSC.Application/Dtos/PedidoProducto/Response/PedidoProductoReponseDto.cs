namespace BSC.Application.Dtos.PedidoProducto.Response;

// PedidoProductoResponseDto.cs
public class PedidoProductoResponseDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
}
