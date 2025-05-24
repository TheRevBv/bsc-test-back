using System.Collections.Generic;
using System.Threading.Tasks;
using BSC.Infrastructure.Sql;

public class ReporteExistenciasService
{
    private readonly ISqlExecutor _sql;

    public ReporteExistenciasService(ISqlExecutor sql)
    {
        _sql = sql;
    }

    public async Task<List<ReporteExistenciaDto>> ObtenerExistencias()
    {
        const string sql = "SELECT * FROM vw_ReporteExistencias";
        return await _sql.ExecuteQueryAsync<ReporteExistenciaDto>(sql);
    }

    public async Task<TotalPedidosResult?> ObtenerTotalPedidos(int usuarioId)
    {
        const string sql = "EXEC sp_PedidosPorUsuario @UsuarioId = {0}";
        return await _sql.ExecuteSingleAsync<TotalPedidosResult>(sql, usuarioId);
    }
}
