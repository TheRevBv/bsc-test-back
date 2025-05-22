using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BSC.Domain.Entities;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("UsuarioId");

            builder.Property(e => e.Correo)
                .IsUnicode(false);

            builder.Property(e => e.Imagen)
                .IsUnicode(false);

            builder.Property(e => e.Contrasena)
                .IsUnicode(false);

            builder.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.TipoAutenticacion)
                .HasMaxLength(15)
                .IsUnicode(false);
        }
    }
}