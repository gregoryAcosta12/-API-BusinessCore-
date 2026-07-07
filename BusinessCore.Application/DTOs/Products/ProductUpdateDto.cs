namespace BusinessCore.Application.DTOs.Products
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public decimal? CostPrice { get; set; }
        public int Stock { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public bool IsActive { get; set; }
    }
}