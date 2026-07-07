namespace BusinessCore.Application.DTOs.Order
{
    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
    }
}