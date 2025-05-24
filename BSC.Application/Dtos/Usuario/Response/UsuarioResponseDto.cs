namespace BSC.Application.Dtos.Usuario.Response
{
    public class UsuarioResponseDto
    {
        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public int? Estado { get; set; }
        public string? Estatus { get; set; }
    }
}