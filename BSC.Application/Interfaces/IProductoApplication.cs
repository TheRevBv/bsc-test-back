using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Producto.Request;
using BSC.Application.Dtos.Producto.Response;
using BSC.Domain.Entities;

namespace BSC.Application.Interfaces
{
    public interface IProductoApplication
    {
        Task<BaseResponse<IEnumerable<ProductoResponseDto>>> ListProductos(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectProductos();
        Task<BaseResponse<ProductoByIdResponseDto>> ProductoById(int productId);
        Task<BaseResponse<Producto?>> RegisterProducto(ProductoRequestDto requestDto);
        Task<BaseResponse<Producto?>> EditProducto(int productId, ProductoRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveProducto(int productId);
        //Task<BaseResponse<IEnumerable<ProductoResponseDto>>> ListProductsByCategory(int categoryId);
        //Task<BaseResponse<PurcharseDetailByIdResponseDto>> ProductDetailById(int productId);
    }
}