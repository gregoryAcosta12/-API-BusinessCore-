namespace BusinessCore.Application.DTOs.Product
{
    public class ProductImageCreateDto
    {
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }
}