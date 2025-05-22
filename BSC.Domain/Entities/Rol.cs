namespace BSC.Domain.Entities
{
    public partial class Rol
    {
        public Rol()
        {
            RolesUsuario = new HashSet<RolUsuario>();
        }

        public int RolId { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<RolUsuario> RolesUsuario { get; set; }
    }

}
