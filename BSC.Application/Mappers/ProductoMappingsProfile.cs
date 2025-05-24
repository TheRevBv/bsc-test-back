using AutoMapper;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Producto.Request;
using BSC.Application.Dtos.Producto.Response;
using BSC.Domain.Entities;
using BSC.Utilities.Static;

namespace BSC.Application.Mappers
{
    public class ProductoMappingsProfile : Profile
    {
        public ProductoMappingsProfile()
        {
            CreateMap<Producto, ProductoResponseDto>()
                .ForMember(x => x.ProductoId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Estatus,
                    x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<Producto, ProductoByIdResponseDto>()
                .ForMember(x => x.ProductoId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<ProductoRequestDto, Producto>();

            CreateMap<Producto, SelectResponse>()
                .ForMember(x => x.Nombre, x => x.MapFrom(y => y.Nombre))
                .ReverseMap();

        }
    }
}