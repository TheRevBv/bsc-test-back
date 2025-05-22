using BSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("PedidoId");

            builder.Property(e => e.Cliente)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(false);

            builder.Property(e => e.FechaPedido)
                   .HasColumnType("datetime");

            builder.HasOne(e => e.Usuario)
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
