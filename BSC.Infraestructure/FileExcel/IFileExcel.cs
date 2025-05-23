using BSC.Utilities.Static;

namespace BSC.Infrastructure.FileExcel
{
        public interface IGenerateExcel
        {
            MemoryStream GenerateToExcel<T>(IEnumerable<T> data, List<TableColumn> columns);
        }
    
}
