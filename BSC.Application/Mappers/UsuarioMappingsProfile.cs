using AutoMapper;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.Usuario.Response;
using BSC.Domain.Entities;
using BSC.Utilities.Static;

namespace BSC.Application.Mappers
{
    public class UsuarioMappingsProfile : Profile
    {
        public UsuarioMappingsProfile()
        {
            CreateMap<Usuario, UsuarioResponseDto>()
                 .ForMember(x => x.UsuarioId, x => x.MapFrom(y => y.Id))
                 .ForMember(x => x.Estatus,
                     x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                 .ReverseMap();

            CreateMap<Usuario, UsuarioByIdResponseDto>()
                .ForMember(x => x.UsuarioId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<UsuarioRequestDto, Usuario>();

            CreateMap<TokenRequestDto, Usuario>();
        }
    }
}