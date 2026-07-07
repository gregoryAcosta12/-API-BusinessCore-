namespace BusinessCore.Application.DTOs.Product
{
    public class ProductVariantCreateDto
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? CostPrice { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int? MinStock { get; set; }
    }
}