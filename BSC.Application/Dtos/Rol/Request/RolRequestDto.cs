using System.ComponentModel.DataAnnotations;

namespace BSC.Application.Dtos.Rol.Request;

public class RolRequestDto
{
    [Required]
    [MaxLength(50)]
    public string? Descripcion { get; set; }
    public int Estado { get; set; } = 1;
}