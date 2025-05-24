public class ReporteExistenciaDto
{
    public int ProductoId { get; set; }
    public string Clave { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public int Existencia { get; set; }
    public int TotalVendido { get; set; }
}
