namespace BSC.Utilities.Static
{
    public class ExcelColumnNames
    {
        public static List<TableColumn> GetColumns(IEnumerable<(string ColumnName, string PropertyName)> columnsProperties)
        {
            var columns = new List<TableColumn>();

            foreach (var (ColumnName, PropertyName) in columnsProperties)
            {
                var column = new TableColumn()
                {
                    Label = ColumnName,
                    PropertyName = PropertyName
                };

                columns.Add(column);
            }

            return columns;
        }

        #region Reporte de Existencia
        public static List<(string ColumnName, string PropertyName)> GetColumnsReporteExistencia()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
                ("ID", "ProductoId"),
                ("Clave del producto", "Clave"),
                ("Nombre del producto", "Nombre"),
                ("Existencia Inicial", "ExistenciaInicial"),
                ("Existencia Actual", "ExistenciaActual"),
                ("Total Vendido", "TotalVendido"),
                ("Precio", "Precio"),
            };

            return columnsProperties;
        }
        #endregion
    }
}