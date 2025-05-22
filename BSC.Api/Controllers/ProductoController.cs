using Microsoft.AspNetCore.Mvc;
using BSC.Application.Interfaces;
using BSC.Application.Commons.Bases.Request;
using BSC.Application.Dtos.Producto.Request;
using BSC.Utilities.Static;

namespace BSC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(IProductoApplication productoApplication) : ControllerBase
    {
        private readonly IProductoApplication _productApplication = productoApplication;

        [HttpGet]
        public async Task<IActionResult> ListProducts([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _productApplication.ListProductos(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsProducts();
                //var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                //return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterProduct([FromForm] ProductoRequestDto requestDto)
        {
            var response = await _productApplication.RegisterProducto(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{productId:int}")]
        public async Task<IActionResult> EditProduct(int productId, [FromForm] ProductoRequestDto requestDto)
        {
            var response = await _productApplication.EditProducto(productId, requestDto);
            return Ok(response);
        }

        [HttpPut("Remove/{productId:int}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var response = await _productApplication.RemoveProducto(productId);
            return Ok(response);
        }
    }
}