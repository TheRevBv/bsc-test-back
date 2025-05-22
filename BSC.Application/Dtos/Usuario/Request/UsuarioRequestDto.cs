using Microsoft.AspNetCore.Http;

namespace BSC.Application.Dtos.Usuario.Request
{
    public class UsuarioRequestDto
    {
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public string? Correo { get; set; }
        public IFormFile? Imagen { get; set; }
        public string? TipoAutenticacion { get; set; }
        public int? Estado { get; set; }
    }
}