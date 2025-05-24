using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.RolUsuario.Request;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.Usuario.Response;
using BSC.Domain.Entities;

namespace BSC.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<BaseResponseAll<IEnumerable<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUsuarios();
        Task<BaseResponse<UsuarioByIdResponseDto>> UsuarioById(int productId);
        Task<BaseResponse<Usuario?>> RegisterUsuario(UsuarioRequestDto requestDto);
        Task<BaseResponse<UsuarioByIdResponseDto?>> EditUsuario(int productId, UsuarioRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveUsuario(int productId);
        Task<BaseResponse<bool>> AsignarRolesUsuario(int usuarioId, AsignarRolesUsuarioDto dto);
    }
}