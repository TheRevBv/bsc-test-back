using BSC.Infrastructure.Persistences.Contexts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BSC.Infrastructure.SqlCore
{
    public class SqlExecutor(BSCContext dbContext) : ISqlExecutor
    {
        private readonly BSCContext _dbContext = dbContext;

        // Obtiene la conexión de EF Core
        private IDbConnection GetConnection() => _dbContext.Database.GetDbConnection();

        public async Task<IEnumerable<T>> QueryAsync<T>(string objectName, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<T>(objectName, parameters, commandType: commandType);
        }

        public async Task<IEnumerable<dynamic>> QueryDynamicAsync(string objectName, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<dynamic>(objectName, parameters, commandType: commandType);
        }

        public async Task<int> ExecuteAsync(string objectName, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(objectName, parameters, commandType: commandType);
        }

        public async Task<T> ExecuteScalarAsync<T>(string objectName, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = GetConnection();
            var result = await connection.ExecuteScalarAsync<T?>(objectName, parameters, commandType: commandType);
            return result ?? throw new InvalidOperationException("El resultado es nulo.");
        }
    }
}