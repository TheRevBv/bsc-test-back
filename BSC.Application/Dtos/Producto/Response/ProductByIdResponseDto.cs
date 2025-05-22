namespace BSC.Application.Dtos.Producto.Response
{
    public class ProductoByIdResponseDto
    {
        public int ProductId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public string? Image { get; set; }
        public decimal UnitSalePrice { get; set; }
        public int CategoryId { get; set; }
        public int State { get; set; }
    }
}