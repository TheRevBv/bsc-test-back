using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Rol.Request;
using BSC.Application.Dtos.Rol.Response;
using BSC.Domain.Entities;

namespace BSC.Application.Interfaces
{
    public interface IRolApplication
    {
        Task<BaseResponseAll<IEnumerable<RolResponseDto>>> ListRoles(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectRoles();
        Task<BaseResponse<RolByIdResponseDto>> RolById(int rolId);
        Task<BaseResponse<Rol>> RegisterRol(RolRequestDto requestDto);
        Task<BaseResponse<Rol>> EditRol(int rolId, RolRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveRol(int rolId);
    }
}