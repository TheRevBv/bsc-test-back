using BSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ProductoId");

            builder.Property(e => e.Clave)
                   .HasMaxLength(11)
                   .IsUnicode(false);

            builder.Property(e => e.Nombre)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(e => e.Existencia)
                   .IsRequired();

            builder.Property(e => e.Precio)
                   .HasColumnType("decimal(10,2)");
        }
    }
}
