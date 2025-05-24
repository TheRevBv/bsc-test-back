using Microsoft.AspNetCore.Mvc;
using BSC.Application.Interfaces;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Dtos.Producto.Request;
using BSC.Utilities.Static;
using Microsoft.AspNetCore.Authorization;
using BSC.Application.Dtos.SqlCore.Request;
using System.Data;

namespace BSC.Api.Controllers
{
    [Route("api/productos")]
    [ApiController]
    [Authorize]
    public class ProductoController(IProductoApplication productoApplication, IGenerateExcelApplication generateExcelApplication, ISqlCoreApplication sqlCoreApplication) : ControllerBase
    {
        private readonly IProductoApplication _productApplication = productoApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication = generateExcelApplication;
        private readonly ISqlCoreApplication _sqlCoreApplication = sqlCoreApplication;

        [HttpGet]
        public async Task<IActionResult> ListProducts([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _productApplication.ListProductos(filters);

            if ((bool)filters.Download!)
            {
                //var columnNames = ExcelColumnNames.GetColumnsProducts();
                //var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                //return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("select")]
        public async Task<IActionResult> ListSelectProducts()
        {
            var response = await _productApplication.ListSelectProductos();
            return Ok(response);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> ProductById(int productId)
        {
            var response = await _productApplication.ProductoById(productId);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> RegisterProduct([FromBody] ProductoRequestDto requestDto)
        {
            var response = await _productApplication.RegisterProducto(requestDto);
            return Ok(response);
        }

        [HttpPut("{productId:int}")]
        public async Task<IActionResult> EditProduct(int productId, [FromBody] ProductoRequestDto requestDto)
        {
            var response = await _productApplication.EditProducto(productId, requestDto);
            return Ok(response);
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var response = await _productApplication.RemoveProducto(productId);
            return Ok(response);
        }

        [HttpGet("reporte-existencias")]
        public async Task<IActionResult> GetReporteExistenciasAsync([FromQuery] bool? download)
        {
            var request = new SqlCoreRequest
            {
                ObjectName = "select * from [dbo].[fxObtenerExistenciaInventarioGeneral]()", // ¡Revisa este nombre!
                CommandType = CommandType.Text,
                ExecutionType = GenericExecutionType.QueryDynamic
            };
            var response = await _sqlCoreApplication.ExecuteAsync(request);

            // Si se pide descarga Y la consulta fue exitosa
            if (response.IsSuccess && download.GetValueOrDefault())
            {
                // Intenta castear a IEnumerable<object>. Esto funcionará para IEnumerable<dynamic>.
                // La variable 'data' será IEnumerable<object>, T se inferirá como 'object'.
                if (response.Data is IEnumerable<object> data && data.Any())
                {
                    // Obtiene las columnas como antes
                    var columnNames = ExcelColumnNames.GetColumnsReporteExistencia();

                    // Llama al método GENÉRICO. ¡Ahora funcionará!
                    var fileBytes = _generateExcelApplication.GenerateToExcel(data, columnNames);

                    var today = DateTime.UtcNow;
                    var fileName = $"reporte_existencias_{today:yyyyMMdd_HHmmss}.xlsx";

                    return File(fileBytes, ContentType.ContentTypeExcel, fileName);
                }
                else
                {
                    // Si se pidió descarga pero no hay datos, informa al usuario.
                    return BadRequest("No hay datos disponibles para generar el reporte.");
                }
            }

            // Si no se pide descarga o hubo error, devuelve la respuesta JSON.
            return Ok(response);
        }
    }
}