using Microsoft.AspNetCore.Http;

namespace BSC.Application.Dtos.Producto.Request
{
    public class ProductoRequestDto
    {
        public string? Clave { get; set; }
        public string? Nombre { get; set; }
        public int Existencia { get; set; }
        public decimal Precio { get; set; }
        public int Estado { get; set; }
    }
}