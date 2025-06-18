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
                .ForMember(x => x.Rol, x => x.MapFrom(y =>
                    y.RolesUsuario.FirstOrDefault() != null
                        ? y.RolesUsuario.First().Rol!.Descripcion
                        : null
                ))
                .ForMember(x => x.RolId, x => x.MapFrom(y =>
                    y.RolesUsuario.FirstOrDefault() != null
                        ? (int?)y.RolesUsuario.First().Rol!.Id
                        : null
                ))
                .ForMember(x => x.Estatus,
                     x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();


            CreateMap<UsuarioRequestDto, Usuario>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<TokenRequestDto, Usuario>();
        }
    }
}