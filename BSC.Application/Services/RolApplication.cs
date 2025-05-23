using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Ordering;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Rol.Request;
using BSC.Application.Dtos.Rol.Response;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;
using WatchDog;

namespace BSC.Application.Services
{
    public class RolApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery) : IRolApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IOrderingQuery _orderingQuery = orderingQuery;

        public async Task<BaseResponseAll<IEnumerable<RolResponseDto>>> ListRoles(BaseFiltersRequest filters)
        {
            var response = new BaseResponseAll<IEnumerable<RolResponseDto>>();

            try
            {
                var roles = _unitOfWork.Rol.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            roles = roles.Where(x => x.Descripcion!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    roles = roles.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    roles = roles.Where(x => x.FechaAlta >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaAlta <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery.Ordering(filters, roles, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.Total = await roles.CountAsync();
                response.Pages = (int)Math.Ceiling((decimal)response.Total / filters.Limit);
                response.Page = filters.Page != 0 ? filters.Page : 1;
                response.Limit = filters.Limit;
                response.Data = _mapper.Map<IEnumerable<RolResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectRoles()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();

            try
            {
                var roles = await _unitOfWork.Rol.GetSelectAsync();

                if (roles is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(roles);
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

        public async Task<BaseResponse<RolByIdResponseDto>> RolById(int rolId)
        {
            var response = new BaseResponse<RolByIdResponseDto>();

            try
            {
                var rol = await _unitOfWork.Rol.GetByIdAsync(rolId);

                if (rol is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<RolByIdResponseDto>(rol);
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

        public async Task<BaseResponse<Rol>> RegisterRol(RolRequestDto requestDto)
        {
            var response = new BaseResponse<Rol>();

            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var rol = _mapper.Map<Rol>(requestDto);
                var createdRol = await _unitOfWork.Rol.RegisterAsync(rol);

                if (createdRol is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                transaction.Commit();
                response.IsSuccess = true;
                response.Data = createdRol;
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

        public async Task<BaseResponse<Rol>> EditRol(int rolId, RolRequestDto requestDto)
        {
            var response = new BaseResponse<Rol>();

            try
            {
                var current = await RolById(rolId);
                if (!current.IsSuccess || current.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var rol = _mapper.Map<Rol>(requestDto);
                rol.Id = rolId;

                var updatedRol = await _unitOfWork.Rol.EditAsync(rol);

                if (updatedRol is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = rol;
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

        public async Task<BaseResponse<bool>> RemoveRol(int rolId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var rol = await RolById(rolId);

                if (!rol.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Rol.RemoveAsync(rolId);

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