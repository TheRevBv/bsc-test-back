namespace BSC.Application.Interfaces
{
    public interface IGenerateExcelApplication
    {
        byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns);
        //MemoryStream GenerateToExcel(IEnumerable<IDictionary<string, object>> data, List<string> columns);
    }
}