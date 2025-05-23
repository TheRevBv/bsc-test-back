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
            CreateMap<PedidoRequestDto, Pedido>().ForMember(dest => dest.ProductosPedido, opt => opt.Ignore());
            CreateMap<PedidoProductoRequestDto, PedidoProducto>();
            CreateMap<Pedido, PedidoResponseDto>();
            CreateMap<Pedido, PedidoByIdResponseDto>().ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.ProductosPedido));
            CreateMap<PedidoProducto, PedidoProductoResponseDto>().ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre));
        }
    }
}
