using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BSC.Application.Commons.Bases.Response;
using BSC.Application.Dtos.Usuario.Request;
using BSC.Application.Dtos.Usuario.Response;
using BSC.Application.Interfaces;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.AppSettings;
using BSC.Utilities.Static;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WatchDog;

namespace BSC.Application.Services
{
    public class AuthApplication(IUnitOfWork unitOfWork,
        IConfiguration configuration, IOptions<AppSettings> appSettings) : IAuthApplication
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;
        private readonly AppSettings _appSettings = appSettings.Value;

        public async Task<BaseResponse<TokenReponseDto>> Login(TokenRequestDto requestDto,
            string? authType)
        {
            var response = new BaseResponse<TokenReponseDto>();

            try
            {
                var user = await _unitOfWork.Usuario.UsuarioByCorreo(requestDto.Correo!);

                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }

                if (!authType.IsNullOrEmpty() && user.TipoAutenticacion != authType)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_AUTH_TYPE_GOOGLE;
                    return response;
                }

                if (PasswordHasher.VerifyPassword(requestDto.Contrasena!, user.Contrasena!))
                {
                    response.IsSuccess = true;
                    response.Data = GenerateToken(user);
                    response.Message = ReplyMessage.MESSAGE_TOKEN;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
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

        public async Task<BaseResponse<TokenReponseDto>> LoginWithGoogle(string credentials,
            string authType)
        {
            var response = new BaseResponse<TokenReponseDto>();

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience =
                    [
                        _appSettings.ClientId!
                    ]
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);
                var user = await _unitOfWork.Usuario.UsuarioByCorreo(payload.Email);

                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_GOOGLE_ERROR;
                    return response;
                }

                if (user.TipoAutenticacion != authType)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_AUTH_TYPE;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = GenerateToken(user);
                response.Message = ReplyMessage.MESSAGE_TOKEN;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        private TokenReponseDto GenerateToken(Usuario user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Correo!),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.NombreUsuario!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Correo!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpireHours"]!)),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            var response = new TokenReponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UsuarioId = user.Id,
                NombreUsuario = user.NombreUsuario!,
                RefreshToken = GenerateRefreshToken()
            };

            return response;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}