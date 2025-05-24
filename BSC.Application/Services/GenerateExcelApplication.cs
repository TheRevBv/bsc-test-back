using BSC.Application.Interfaces;
using BSC.Infrastructure.FileExcel;
using BSC.Utilities.Static;

namespace BSC.Application.Services
{

    public class GenerateExcelApplication(IGenerateExcel generateExcel) : IGenerateExcelApplication
    {
        private readonly IGenerateExcel _generateExcel = generateExcel;

        public byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns)
        {
            var excelColumns = ExcelColumnNames.GetColumns(columns);
            var memoryStreamExcel = _generateExcel.GenerateToExcel(data, excelColumns);
            var fileBytes = memoryStreamExcel.ToArray();

            return fileBytes;
        }
    }
}