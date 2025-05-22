using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Usuario.Request;

namespace BSC.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<BaseResponse<bool>> RegisterUser(UsuarioRequestDto requestDto);
    }
}