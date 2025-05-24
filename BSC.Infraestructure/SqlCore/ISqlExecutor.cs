namespace BSC.Infrastructure.SqlCore
{
    // Define cómo se pasarán los parámetros (puede ser un Dictionary o un objeto anónimo)
    public interface IDbParameters : IDictionary<string, object> { }

    public class DynamicDbParameters : Dictionary<string, object>, IDbParameters { }

    public interface ISqlExecutor
    {
        /// <summary>
        /// Ejecuta una consulta (SP, Vista, TVF, SQL) y mapea los resultados a un tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo al que se mapearán los resultados.</typeparam>
        /// <param name="objectName">Nombre del SP, Vista, TVF o consulta SQL completa.</param>
        /// <param name="parameters">Parámetros (opcional).</param>
        /// <param name="commandType">Tipo de comando (StoredProcedure, Text).</param>
        /// <returns>Una colección de objetos T.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string objectName, object? parameters = null, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// Ejecuta una consulta y devuelve los resultados como una lista de objetos dinámicos.
        /// </summary>
        Task<IEnumerable<dynamic>> QueryDynamicAsync(string objectName, object? parameters = null, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// Ejecuta un comando (SP, SQL) que no devuelve un conjunto de resultados.
        /// </summary>
        /// <returns>El número de filas afectadas.</returns>
        Task<int> ExecuteAsync(string objectName, object? parameters = null, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// Ejecuta una consulta (SP, SQL, UDF) y devuelve el primer valor de la primera fila.
        /// </summary>
        Task<T> ExecuteScalarAsync<T>(string objectName, object? parameters = null, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure);
    }
}