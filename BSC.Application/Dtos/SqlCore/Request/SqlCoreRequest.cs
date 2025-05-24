// Tipo de operación a realizar
using System.Data;

namespace BSC.Application.Dtos.SqlCore.Request;

public enum GenericExecutionType
{
    Query,       // Devuelve una lista de objetos (T o dynamic)
    QueryDynamic, // Devuelve una lista de objetos dynamic
    Execute,     // Ejecuta un comando (devuelve filas afectadas)
    Scalar       // Devuelve un único valor
}

// Solicitud genérica
public class SqlCoreRequest
{
    /// <summary>
    /// Nombre del objeto de BD (SP, Vista, TVF) o SQL (si se permite).
    /// </summary>
    public string ObjectName { get; set; } = string.Empty;

    /// <summary>
    /// Parámetros para la ejecución.
    /// </summary>
    public IDictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Tipo de comando (StoredProcedure por defecto, Text para SQL/Vistas/TVFs).
    /// </summary>
    public CommandType CommandType { get; set; } = CommandType.StoredProcedure;

    /// <summary>
    /// Qué tipo de ejecución realizar.
    /// </summary>
    public GenericExecutionType ExecutionType { get; set; }

    /// <summary>
    /// Opcional: El nombre del tipo esperado si se usa Query<T>.
    /// Podría usarse con reflexión, pero aumenta la complejidad.
    /// A menudo, es mejor tener servicios específicos o usar QueryDynamic.
    /// </summary>
    public string? ExpectedTypeName { get; set; }
}