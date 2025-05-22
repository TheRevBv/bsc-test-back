namespace BSC.Domain.Entities
{

    public partial class RolUsuario
    {
        public int RolUsuarioId { get; set; }
        public int? RolId { get; set; }
        public int? UsuarioId { get; set; }
        public int? Estado { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
