using BSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class PedidoProductoConfiguration : IEntityTypeConfiguration<PedidoProducto>
    {
        public void Configure(EntityTypeBuilder<PedidoProducto> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("PedidoProductoId");

            builder.Property(e => e.Cantidad)
                   .IsRequired();

            builder.HasOne(e => e.Pedido)
                   .WithMany(p => p.ProductosPedido)
                   .HasForeignKey(e => e.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Producto)
                   .WithMany(p => p.ProductosPedido)
                   .HasForeignKey(e => e.ProductoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
