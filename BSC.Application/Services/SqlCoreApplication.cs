using BSC.Application.Dtos.SqlCore.Reponse;
using BSC.Application.Dtos.SqlCore.Request;
using BSC.Application.Interfaces;
using BSC.Infrastructure.SqlCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BSC.Application.Services
{
    public class SqlCoreApplication(
        ISqlExecutor dbExecutor,
        ILogger<SqlCoreApplication> logger) : ISqlCoreApplication
    {
        private readonly ISqlExecutor _dbExecutor = dbExecutor;
        private readonly ILogger<SqlCoreApplication> _logger = logger;

        public async Task<SqlCoreResponseDto> ExecuteAsync(SqlCoreRequest request)
        {
            try
            {
                // 1. Validación de la Solicitud
                if (request == null || string.IsNullOrWhiteSpace(request.ObjectName))
                {
                    return SqlCoreResponseDto.Failure("La solicitud o el nombre del objeto no pueden ser nulos.");
                }

                // 2. (MUY IMPORTANTE) Validación de Seguridad y Autorización
                // ¿Está el usuario autenticado?
                // ¿Tiene permiso para ejecutar 'request.ObjectName'?
                // ¿Es 'request.ObjectName' un objeto permitido en una lista blanca?
                // ¡NUNCA permitas CommandType.Text con SQL construido por el usuario!
                if (!IsRequestAllowed(request))
                {
                    _logger.LogWarning($"Intento de ejecución no permitida: {request.ObjectName}");
                    return SqlCoreResponseDto.Failure("Operación no permitida.");
                }

                _logger.LogInformation($"Ejecutando solicitud genérica: {request.ObjectName}");

                // 3. Ejecución según el tipo
                object? resultData = null;
                switch (request.ExecutionType)
                {
                    case GenericExecutionType.QueryDynamic:
                        resultData = await _dbExecutor.QueryDynamicAsync(request.ObjectName, request.Parameters, request.CommandType);
                        break;
                    case GenericExecutionType.Execute:
                        resultData = await _dbExecutor.ExecuteAsync(request.ObjectName, request.Parameters, request.CommandType);
                        break;
                    case GenericExecutionType.Scalar:
                        // Para escalar, necesitamos saber el tipo, o devolver object.
                        resultData = await _dbExecutor.ExecuteScalarAsync<object>(request.ObjectName, request.Parameters, request.CommandType);
                        break;
                    case GenericExecutionType.Query:
                        // Este caso es mejor manejarlo con el método ExecuteQueryAsync<T>
                        return SqlCoreResponseDto.Failure("Para consultas tipadas, use ExecuteQueryAsync<T>.");
                    default:
                        return SqlCoreResponseDto.Failure("Tipo de ejecución no válido.");
                }

                return SqlCoreResponseDto.Success(resultData);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error ejecutando solicitud genérica '{request?.ObjectName}': {ex.Message}");
                return SqlCoreResponseDto.Failure($"Ocurrió un error interno: {ex.Message}");
            }
        }

        public async Task<SqlCoreResponseDto> ExecuteQueryAsync<T>(SqlCoreRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ObjectName) || request.ExecutionType != GenericExecutionType.Query)
                {
                    return SqlCoreResponseDto.Failure("Solicitud inválida para ExecuteQueryAsync<T>.");
                }

                if (!IsRequestAllowed(request))
                {
                    _logger.LogWarning($"Intento de ejecución no permitida (Query<T>): {request.ObjectName}");
                    return SqlCoreResponseDto.Failure("Operación no permitida.");
                }

                _logger.LogInformation($"Ejecutando QueryAsync<{typeof(T).Name}>: {request.ObjectName}");

                var resultData = await _dbExecutor.QueryAsync<T>(request.ObjectName, request.Parameters, request.CommandType);
                return SqlCoreResponseDto.Success(resultData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error ejecutando QueryAsync<{typeof(T).Name}> '{request?.ObjectName}': {ex.Message}");
                return SqlCoreResponseDto.Failure($"Ocurrió un error interno: {ex.Message}");
            }
        }


        /// <summary>
        /// (PLACEHOLDER - MUY IMPORTANTE)
        /// Aquí debes implementar tu lógica de seguridad.
        /// Esto podría implicar verificar roles de usuario, consultar una
        /// base de datos/configuración de SPs/Vistas permitidas, etc.
        /// ¡NO DEJES ESTO ABIERTO!
        /// </summary>
        private bool IsRequestAllowed(SqlCoreRequest request)
        {
            // Ejemplo básico: Prohibir SQL crudo si no viene de una fuente 100% confiable
            if (request.CommandType == CommandType.Text && !IsSafeTextCommand(request.ObjectName))
            {
                _logger.LogError($"Intento de ejecutar CommandType.Text no seguro: {request.ObjectName}");
                return false;
            }

            // Ejemplo: Lista blanca de SPs permitidos
            //var allowedSPs = new HashSet<string> { "sp_GetProductsByCategory", "sp_UpdateOrderStatus", "sp_GetTotalOrderCount" };
            //var allowedViews = new HashSet<string> { "V_CustomerSales" };


            // Aquí irían más chequeos (roles, etc.)
            return true; // ¡Implementa lógica real aquí!
        }

        private bool IsSafeTextCommand(string sql)
        {
            // Implementa lógica para verificar si es una consulta segura a una vista/TVF conocida.
            // ¡Esto es difícil y riesgoso! Es mucho más seguro usar listas blancas.
            //return sql.TrimStart().ToUpper().StartsWith("SELECT * FROM V_"); // Ejemplo MUY simplista y NO seguro.
            return true;
        }
    }
}