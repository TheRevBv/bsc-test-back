using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BSC.Domain.Entities;

namespace BSC.Infrastructure.Persistences.Contexts.Configurations
{
    public class RolUsuarioConfiguration : IEntityTypeConfiguration<RolUsuario>
    {
        public void Configure(EntityTypeBuilder<RolUsuario> builder)
        {

            builder.HasOne(d => d.Rol)
                .WithMany(p => p.RolesUsuario)
                .HasForeignKey(d => d.RolId);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.RolesUsuario)
                .HasForeignKey(d => d.UsuarioId);
        }
    }
}