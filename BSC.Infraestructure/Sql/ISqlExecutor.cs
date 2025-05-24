using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSC.Infrastructure.Sql
{
    public interface ISqlExecutor
    {
        Task<List<T>> ExecuteQueryAsync<T>(string sql, params object[] parameters) where T : class, new();
        Task<T?> ExecuteSingleAsync<T>(string sql, params object[] parameters) where T : class, new();
    }
}