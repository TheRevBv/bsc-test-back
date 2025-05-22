using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Domain.Entities;

namespace BSC.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<BaseResponse<Usuario?>> RegisterUser(UsuarioRequestDto requestDto);
    }
}