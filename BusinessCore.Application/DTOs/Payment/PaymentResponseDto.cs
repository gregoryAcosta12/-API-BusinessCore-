using System;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public DateTime PaymentDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}