namespace BSC.Application.Dtos.RolUsuario.Request;

public class AsignarRolesUsuarioDto
{
    public int UsuarioId { get; set; }
    public List<int> RolesIds { get; set; } = [];
}
