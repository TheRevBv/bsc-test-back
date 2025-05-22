namespace BSC.Domain.Entities
{
    public partial class Usuario : BaseEntity
    {
        public Usuario()
        {
            RolesUsuario = new HashSet<RolUsuario>();
            Pedidos = new HashSet<Pedido>();
        }

        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public string? Correo { get; set; }
        public string? Imagen { get; set; }
        public string? TipoAutenticacion { get; set; }

        public virtual ICollection<RolUsuario> RolesUsuario { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
