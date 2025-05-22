using System.ComponentModel.DataAnnotations;

namespace BSC.Application.Dtos.Pedido.Request
{
    public class PedidoRequestDto
    {
        [Required]
        public string Cliente { get; set; } = null!;

        [Required]
        public DateTime FechaPedido { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public List<PedidoProductoRequestDto> Productos { get; set; } = [];
    }
}
