namespace BSC.Application.Dtos.Producto.Reports
{
    public class RptExistenciaInventarioDto
    {
        public int ProductoId { get; set; }
        public string Clave { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int ExistenciaInicial { get; set; }
        public int TotalVendido { get; set; }
        public int ExistenciaActual { get; set; }
        public decimal Precio { get; set; }
    }

}
