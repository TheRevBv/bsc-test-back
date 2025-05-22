
namespace BSC.Domain.Entities
{
    public partial class Pedido : BaseEntity
    {
        public Pedido()
        {
            ProductosPedido = new HashSet<PedidoProducto>();
        }

        public int UsuarioId { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime FechaPedido { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<PedidoProducto> ProductosPedido { get; set; }
    }
}
