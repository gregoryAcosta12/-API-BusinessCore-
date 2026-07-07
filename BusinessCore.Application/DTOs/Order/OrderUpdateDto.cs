using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Order
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string TrackingNumber { get; set; }
        public string Notes { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string ShippingMethod { get; set; }
    }
}