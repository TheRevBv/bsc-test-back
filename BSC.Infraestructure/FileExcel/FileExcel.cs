using ClosedXML.Excel;
using BSC.Utilities.Static;

namespace BSC.Infrastructure.FileExcel
{
    public class GenerateExcel : IGenerateExcel
    {
        public MemoryStream GenerateToExcel<T>(IEnumerable<T> data, List<TableColumn> columns)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Listado");

            for (int i = 0; i < columns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = columns[i].Label;
            }

            var rowIndex = 2;

            foreach (var item in data)
            {

                if (item is IDictionary<string, object> dict)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        string propName = columns[i].PropertyName!;
                        object? propertyValue = null;

                        var key = dict.Keys.FirstOrDefault(k => k.Equals(propName, StringComparison.OrdinalIgnoreCase));

                        if (key != null)
                        {
                            dict.TryGetValue(key, out propertyValue);
                        }

                        worksheet.Cell(rowIndex, i + 1).Value = XLCellValue.FromObject(propertyValue);
                    }
                }
                else
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var propertyValue = typeof(T).GetProperty(columns[i].PropertyName!)?.GetValue(item);
                        worksheet.Cell(rowIndex, i + 1).Value = XLCellValue.FromObject(propertyValue);
                    }
                }

                rowIndex++;
            }

            worksheet.ColumnsUsed().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}