using System;

namespace BusinessCore.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } 
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string Notes { get; set; }
        public string GatewayResponse { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Order Order { get; set; }
    }
}