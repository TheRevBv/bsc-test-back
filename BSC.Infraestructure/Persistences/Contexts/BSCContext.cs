using BSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BSC.Infrastructure.Persistences.Contexts
{
    public partial class BSCContext : DbContext
    {
        public BSCContext()
        {
        }

        public BSCContext(DbContextOptions<BSCContext> options)
            : base(options)
        {
        }

        // DbSets: Aquí se agregan todas las entidades del modelo
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<RolUsuario> RolesUsuario { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<PedidoProducto> PedidosProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            // Aplica configuraciones si usas IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
