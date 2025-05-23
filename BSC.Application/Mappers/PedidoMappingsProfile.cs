using AutoMapper;
using BSC.Application.Dtos.Pedido.Request;
using BSC.Application.Dtos.Pedido.Response;
using BSC.Application.Dtos.PedidoProducto.Request;
using BSC.Application.Dtos.PedidoProducto.Response;
using BSC.Domain.Entities;

namespace BSC.Application.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoRequestDto, Pedido>()
                .ForMember(dest => dest.ProductosPedido, opt => opt.Ignore());

            CreateMap<PedidoProductoRequestDto, PedidoProducto>();

            CreateMap<Pedido, PedidoResponseDto>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.ProductosPedido));

            CreateMap<PedidoProducto, PedidoProductoSimpleDto>()
                .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.ProductoId))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Producto.Precio));

            CreateMap<PedidoProducto, PedidoProductoResponseDto>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<Pedido, PedidoByIdResponseDto>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.ProductosPedido));

        }
    }
}
