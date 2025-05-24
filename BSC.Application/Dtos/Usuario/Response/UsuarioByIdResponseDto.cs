
namespace BSC.Application.Dtos.Usuario.Response
{
    public class UsuarioByIdResponseDto : UsuarioResponseDto
    {
        public string? Rol { get; set; }
        public int RolId { get; set; }
    }
}