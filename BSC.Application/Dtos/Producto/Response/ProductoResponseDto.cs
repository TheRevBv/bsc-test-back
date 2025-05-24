namespace BSC.Application.Dtos.Producto.Response
{
    public class ProductoResponseDto
    {
        public int ProductoId { get; set; }
        public string? Clave { get; set; }
        public string? Nombre { get; set; }
        public int? Existencia { get; set; }
        public decimal Precio { get; set; }
        public DateTime? FechaAlta { get; set; }
        public int Estado { get; set; }
        public string? Estatus { get; set; }
    }
}