using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.Usuario.Response;

namespace BSC.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<BaseResponse<TokenReponseDto>> Login(TokenRequestDto requestDto, string authType);
        Task<BaseResponse<TokenReponseDto>> LoginWithGoogle(string credentials, string authType);
    }
}