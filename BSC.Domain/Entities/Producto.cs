using System.Text.Json.Serialization;

namespace BSC.Domain.Entities
{
    public partial class Producto : BaseEntity
    {
        public Producto()
        {
            ProductosPedido = new HashSet<PedidoProducto>();
        }

        public string? Clave { get; set; }
        public string? Nombre { get; set; }
        public int Existencia { get; set; }
        public decimal? Precio { get; set; }

        [JsonIgnore]
        public virtual ICollection<PedidoProducto> ProductosPedido { get; set; }
    }
}