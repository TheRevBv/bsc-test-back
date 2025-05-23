using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Ordering;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Producto.Request;
using BSC.Application.Dtos.Producto.Response;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;
using WatchDog;
using BSC.Infrastructure.SqlCore;

namespace BSC.Application.Services
{
    public class ProductoApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, ISqlExecutor sqlExecutor) : IProductoApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IOrderingQuery _orderingQuery = orderingQuery;
        private readonly ISqlExecutor _sqlExecutor = sqlExecutor;

        public async Task<BaseResponseAll<IEnumerable<ProductoResponseDto>>> ListProductos(BaseFiltersRequest filters)
        {
            var response = new BaseResponseAll<IEnumerable<ProductoResponseDto>>();

            try
            {
                var products = _unitOfWork.Producto.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            products = products.Where(x => x.Clave!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            products = products.Where(x => x.Nombre!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    products = products.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    products = products.Where(x => x.FechaAlta >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaAlta <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery.Ordering(filters, products, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.Total = await products.CountAsync();
                response.Pages = (int)Math.Ceiling((decimal)response.Total / filters.Limit);
                response.Page = filters.Page != 0 ? filters.Page : 1;
                response.Limit = filters.Limit;
                response.Data = _mapper.Map<IEnumerable<ProductoResponseDto>>(items);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectProductos()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();

            try
            {
                var products = await _unitOfWork.Producto.GetSelectAsync();

                if (products is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(products);
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<ProductoByIdResponseDto>> ProductoById(int productId)
        {
            var response = new BaseResponse<ProductoByIdResponseDto>();

            try
            {
                var product = await _unitOfWork.Producto.GetByIdAsync(productId);

                if (product is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<ProductoByIdResponseDto>(product);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<Producto?>> RegisterProducto(ProductoRequestDto requestDto)
        {
            var response = new BaseResponse<Producto?>();

            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var product = _mapper.Map<Producto>(requestDto);

                // if (requestDto.Image is not null)
                //     product.Image = await _fileStorage.SaveFile(AzureContainers.PRODUCTS, requestDto.Image);

                var createdProduct = await _unitOfWork.Producto.RegisterAsync(product);

                if (createdProduct is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                transaction.Commit();
                response.IsSuccess = true;
                response.Data = createdProduct;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<Producto?>> EditProducto(int productId, ProductoRequestDto requestDto)
        {
            var response = new BaseResponse<Producto?>();

            try
            {
                var current = await ProductoById(productId);
                if (!current.IsSuccess || current.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var product = _mapper.Map<Producto>(requestDto);
                product.Id = productId;

                // if (requestDto.Image is not null)
                //     product.Image = await _fileStorage.EditFile(AzureContainers.PRODUCTS, requestDto.Image, current.Data.Image!);
                // else
                //     product.Image = current.Data.Image;

                var updatedProduct = await _unitOfWork.Producto.EditAsync(product);

                if (updatedProduct is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = product;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RemoveProducto(int productId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var product = await ProductoById(productId);

                response.Data = await _unitOfWork.Producto.RemoveAsync(productId);

                //await _fileStorage.RemoveFile(product.Data!.Image!, AzureContainers.PRODUCTS);

                if (!response.Data)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}