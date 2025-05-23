namespace BSC.Domain.Entities
{
    public partial class Rol : BaseEntity
    {
        public Rol()
        {
            RolesUsuario = new HashSet<RolUsuario>();
        }
        public string? Descripcion { get; set; }
        public virtual ICollection<RolUsuario> RolesUsuario { get; set; }
    }

}
