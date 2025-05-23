
using BSC.Application.Dtos.SqlCore.Reponse;
using BSC.Application.Dtos.SqlCore.Request;

namespace BSC.Application.Interfaces
{
    public interface ISqlCoreApplication
    {
        /// <summary>
        /// Ejecuta una solicitud genérica a la base de datos de forma controlada.
        /// </summary>
        /// <param name="request">La solicitud con los detalles de la operación.</param>
        /// <returns>Una respuesta genérica con los resultados o un error.</returns>
        Task<SqlCoreResponseDto> ExecuteAsync(SqlCoreRequest request);

        /// <summary>
        /// Ejecuta una consulta genérica y la mapea a un tipo T.
        /// Requiere que T sea conocido en tiempo de compilación por el llamador.
        /// </summary>
        Task<SqlCoreResponseDto> ExecuteQueryAsync<T>(SqlCoreRequest request);
    }

}
