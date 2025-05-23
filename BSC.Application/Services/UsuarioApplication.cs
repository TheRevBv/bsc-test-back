using AutoMapper;
using Microsoft.Extensions.Configuration;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;

namespace BSC.Application.Services
{
    public class UsuarioApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : IUsuarioApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;
        //private readonly IAzureStorage _azureStorage = azureStorage;

        public async Task<BaseResponse<Usuario?>> RegisterUser(UsuarioRequestDto requestDto)
        {
            var response = new BaseResponse<Usuario?>();
            var account = _mapper.Map<Usuario>(requestDto);
            account.Contrasena = PasswordHasher.HashPassword(account.Contrasena!);

            if (requestDto.Imagen != null || requestDto.Imagen?.Length > 0)
            {
                // account.Imagen = await _azureStorage.SaveFile(AzureContainers.USERS, requestDto.Image);
            }

            var createdUser = await _unitOfWork.Usuario.RegisterAsync(account);

            response.Data = createdUser;

            if (createdUser is not null)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }


        public async Task<Usuario> GetUserByEmail(string email)
        {
            return await _unitOfWork.Usuario.UsuarioByCorreo(email);
        }
    }
}