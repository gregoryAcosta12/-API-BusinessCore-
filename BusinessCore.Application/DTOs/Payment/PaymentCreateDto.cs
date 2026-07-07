using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Payment
{
    public class PaymentCreateDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string Notes { get; set; }
    }
}