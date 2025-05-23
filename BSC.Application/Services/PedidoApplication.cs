using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Ordering;
using BSC.Application.Dtos.Pedido.Request;
using BSC.Application.Dtos.Pedido.Response;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;
using WatchDog;
using Microsoft.EntityFrameworkCore.Query;

namespace BSC.Application.Services
{
    public class PedidoApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery) : IPedidoApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IOrderingQuery _orderingQuery = orderingQuery;

        public async Task<BaseResponseAll<IEnumerable<PedidoResponseDto>>> ListPedidos(BaseFiltersRequest filters)
        {
            var response = new BaseResponseAll<IEnumerable<PedidoResponseDto>>();

            try
            {
                var pedidosQuery = _unitOfWork.Pedido.GetAllQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            pedidosQuery = pedidosQuery.Where(x => x.Cliente!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    pedidosQuery = pedidosQuery.Where(x => x.FechaPedido >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaPedido <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                var pedidos = pedidosQuery
                    .Include(p => p.ProductosPedido)
                        .ThenInclude(pp => pp.Producto);

                filters.Sort ??= "Id";

                var items = await _orderingQuery.Ordering(filters, pedidos, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.Total = await pedidos.CountAsync();
                response.Pages = (int)Math.Ceiling((decimal)response.Total / filters.Limit);
                response.Page = filters.Page != 0 ? filters.Page : 1;
                response.Limit = filters.Limit;
                response.Data = _mapper.Map<IEnumerable<PedidoResponseDto>>(items);
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

        public async Task<BaseResponse<PedidoByIdResponseDto>> PedidoById(int pedidoId)
        {
            var response = new BaseResponse<PedidoByIdResponseDto>();

            try
            {
                var pedido = await _unitOfWork.Pedido.GetByIdAsync(pedidoId);

                if (pedido is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<PedidoByIdResponseDto>(pedido);
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

        public async Task<BaseResponse<Pedido?>> RegisterPedido(PedidoRequestDto requestDto)
        {
            var response = new BaseResponse<Pedido?>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var pedido = _mapper.Map<Pedido>(requestDto);
                pedido.ProductosPedido = [];

                foreach (var item in requestDto.Productos)
                {
                    var producto = await _unitOfWork.Producto.GetByIdAsync(item.ProductoId);

                    if (producto == null)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = $"El producto con ID {item.ProductoId} no existe.";
                        return response;
                    }
                    if (producto.Existencia < item.Cantidad)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = $"El producto con clave {producto.Clave} no tiene suficiente existencia.";
                        return response;
                    }

                    producto.Existencia -= item.Cantidad;
                    await _unitOfWork.Producto.EditAsync(producto);

                    pedido.ProductosPedido.Add(new PedidoProducto
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad
                    });
                }

                var created = await _unitOfWork.Pedido.RegisterAsync(pedido);

                if (created is null)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                transaction.Commit();
                response.IsSuccess = true;
                response.Data = created;
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

        public async Task<BaseResponse<Pedido?>> EditPedido(int pedidoId, PedidoRequestDto requestDto)
        {
            var response = new BaseResponse<Pedido?>();

            try
            {
                var current = await PedidoById(pedidoId);
                if (!current.IsSuccess || current.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var pedido = _mapper.Map<Pedido>(requestDto);
                pedido.Id = pedidoId;

                var updated = await _unitOfWork.Pedido.EditAsync(pedido);

                if (updated is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = updated;
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

        public async Task<BaseResponse<bool>> RemovePedido(int pedidoId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                response.Data = await _unitOfWork.Pedido.RemoveAsync(pedidoId);

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
