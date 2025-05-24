using System.ComponentModel.DataAnnotations;

namespace BSC.Application.Dtos.Producto.Request
{
    public class ProductoRequestDto
    {
        [Required]
        public string? Clave { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public int Existencia { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public int Estado { get; set; } = 1;
    }
}