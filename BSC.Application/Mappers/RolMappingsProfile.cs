using AutoMapper;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Rol.Request;
using BSC.Application.Dtos.Rol.Response;
using BSC.Domain.Entities;
using BSC.Utilities.Static;

namespace BSC.Application.Mappers
{
    public class RolMappingsProfile : Profile
    {
        public RolMappingsProfile()
        {
            CreateMap<Rol, RolResponseDto>()
                .ForMember(x => x.RolId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Estatus,
                    x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<Rol, RolByIdResponseDto>()
                .ForMember(x => x.RolId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Estatus,
                    x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<RolRequestDto, Rol>()
                .ForMember(x => x.Estado, x => x.MapFrom(y => y.Estado))
                .ForMember(x => x.Descripcion, x => x.MapFrom(y => y.Descripcion))
                .ReverseMap();

            CreateMap<Rol, SelectResponse>()
                .ForMember(x => x.Nombre, x => x.MapFrom(y => y.Descripcion))
                .ReverseMap();
        }
    }
}