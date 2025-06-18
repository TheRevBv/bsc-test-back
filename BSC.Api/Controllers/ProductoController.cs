using BSC.Application.Commons.Bases.Request;
using BSC.Application.Dtos.Producto.Reports;
using BSC.Application.Dtos.Producto.Request;
using BSC.Application.Dtos.SqlCore.Request;
using BSC.Application.Interfaces;
using BSC.Utilities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var request = new SqlCoreRequest
                {
                    ObjectName = "select * from [dbo].[fxObtenerExistenciaInventarioGeneral]()",
                    CommandType = CommandType.Text,
                    ExecutionType = GenericExecutionType.Query
                };

                var response = await _sqlCoreApplication.ExecuteQueryAsync<RptExistenciaInventarioDto>(request);

                if (response.IsSuccess && download.GetValueOrDefault())
                {
                    if (response.Data is List<RptExistenciaInventarioDto> dataList && dataList.Count != 0)
                    {
                        var columnNames = ExcelColumnNames.GetColumnsReporteExistencia();
                        var fileBytes = _generateExcelApplication.GenerateToExcel<RptExistenciaInventarioDto>(dataList, columnNames);
                        var fileName = $"reporte_existencias_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

                        return File(fileBytes, ContentType.ContentTypeExcel, fileName);
                    }


                    return BadRequest("No hay datos disponibles para generar el reporte.");
                }

                return Ok(response);
            }   
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno generando el archivo Excel.");
            }
        }

    }
}