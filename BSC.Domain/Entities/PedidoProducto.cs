namespace BSC.Domain.Entities
{
    public partial class PedidoProducto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
