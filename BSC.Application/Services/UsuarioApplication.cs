using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Commons.Ordering;
using BSC.Application.Commons.Select.Response;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.Usuario.Response;
using BSC.Application.Dtos.RolUsuario.Request;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;
using WatchDog;

namespace BSC.Application.Services
{
    public class UsuarioApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IOrderingQuery orderingQuery) : IUsuarioApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;
        private readonly IOrderingQuery _orderingQuery = orderingQuery;

        public async Task<BaseResponse<Usuario?>> RegisterUsuario(UsuarioRequestDto requestDto)
        {
            var response = new BaseResponse<Usuario?>();
            var account = _mapper.Map<Usuario>(requestDto);
            account.Contrasena = PasswordHasher.HashPassword(account.Contrasena!);

            var createdUser = await _unitOfWork.Usuario.RegisterAsync(account);

            response.Data = createdUser;
            response.IsSuccess = createdUser is not null;
            response.Message = createdUser is not null ? ReplyMessage.MESSAGE_SAVE : ReplyMessage.MESSAGE_FAILED;
            return response;
        }

        public async Task<Usuario> GetUserByEmail(string email)
        {
            return await _unitOfWork.Usuario.UsuarioByCorreo(email);
        }

        public async Task<BaseResponseAll<IEnumerable<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponseAll<IEnumerable<UsuarioResponseDto>>();

            try
            {
                var query = _unitOfWork.Usuario.GetAllQueryable();

                if (filters.NumFilter != null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(u => u.Correo!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            query = query.Where(u => u.NombreUsuario!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter != null)
                {
                    query = query.Where(u => u.Estado == filters.StateFilter);
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    query = query.Where(u => u.FechaAlta >= Convert.ToDateTime(filters.StartDate) && u.FechaAlta <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                var usuarios = await _orderingQuery.Ordering(filters, query, !filters.Download!.Value).ToListAsync();

                response.IsSuccess = true;
                response.Total = await query.CountAsync();
                response.Pages = (int)Math.Ceiling((decimal)response.Total / filters.Limit);
                response.Page = filters.Page != 0 ? filters.Page : 1;
                response.Limit = filters.Limit;
                response.Data = _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
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

        public async Task<BaseResponse<UsuarioByIdResponseDto>> UsuarioById(int id)
        {
            var response = new BaseResponse<UsuarioByIdResponseDto>();

            try
            {
                var usuario = await _unitOfWork.Usuario.GetByIdAsync(id);

                if (usuario is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<UsuarioByIdResponseDto>(usuario);
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

        public async Task<BaseResponse<Usuario?>> EditUsuario(int id, UsuarioRequestDto dto)
        {
            var response = new BaseResponse<Usuario?>();

            try
            {
                var usuario = await _unitOfWork.Usuario.GetByIdAsync(id);
                if (usuario is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var updated = _mapper.Map(dto, usuario);
                updated.Id = id;

                var result = await _unitOfWork.Usuario.EditAsync(updated);

                response.IsSuccess = result is not null;
                response.Data = result;
                response.Message = result is not null ? ReplyMessage.MESSAGE_UPDATE : ReplyMessage.MESSAGE_FAILED;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RemoveUsuario(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                response.Data = await _unitOfWork.Usuario.RemoveAsync(id);
                response.IsSuccess = response.Data;
                response.Message = response.Data ? ReplyMessage.MESSAGE_DELETE : ReplyMessage.MESSAGE_FAILED;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> AsignarRolesUsuario(int usuarioId, AsignarRolesUsuarioDto dto)
        {
            var response = new BaseResponse<bool>();

            var usuario = await _unitOfWork.Usuario.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }

            var actuales = await _unitOfWork.RolUsuario.GetByUsuarioIdAsync(usuarioId);
            _unitOfWork.RolUsuario.RemoveRange(actuales);

            var nuevos = dto.RolesIds.Select(rolId => new RolUsuario
            {
                UsuarioId = usuarioId,
                RolId = rolId,
                Estado = 1
            });

            await _unitOfWork.RolUsuario.AddRangeAsync(nuevos);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Data = true;
            response.Message = "Roles asignados correctamente";
            return response;
        }

        public Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUsuarios()
        {
            throw new NotImplementedException();
        }
    }
}