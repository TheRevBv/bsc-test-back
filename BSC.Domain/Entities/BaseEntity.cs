namespace BSC.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int UsuarioAltaId { get; set; }
        public DateTime FechaAlta { get; set; }
        public int? UsuarioModId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int Estado { get; set; } // 1 = Activo, 0 = Inactivo
    }
}