using BSC.Infrastructure.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSC.Infrastructure.Sql
{
    public class SqlExecutor(BSCContext context) : ISqlExecutor
    {
        private readonly BSCContext _context = context;

        public async Task<List<T>> ExecuteQueryAsync<T>(string sql, params object[] parameters) where T : class, new()
        {
            return await _context.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().ToListAsync();
        }

        public async Task<T?> ExecuteSingleAsync<T>(string sql, params object[] parameters) where T : class, new()
        {
            return await _context.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}