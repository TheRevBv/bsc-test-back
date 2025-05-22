using AutoMapper;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Domain.Entities;

namespace BSC.Application.Mappers
{
    public class UsuarioMappingsProfile : Profile
    {
        public UsuarioMappingsProfile()
        {
            CreateMap<UsuarioRequestDto, Usuario>();
            CreateMap<TokenRequestDto, Usuario>();
        }
    }
}