using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BSC.Domain.Entities;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {

            builder.Property(e => e.Id).HasColumnName("RolId");

            builder.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
        }
    }
}