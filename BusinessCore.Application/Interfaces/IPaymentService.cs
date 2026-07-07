using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Payment;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.Interfaces
{
  
    public interface IPaymentService
    {
       
        Task<PaymentResponseDto> GetByIdAsync(int id);
        Task<PaymentResponseDto> GetByTransactionIdAsync(string transactionId);
        Task<IEnumerable<PaymentResponseDto>> GetOrderPaymentsAsync(int orderId);
        Task<PagedResultDto<PaymentResponseDto>> GetPagedAsync(PaymentFilterDto filter);
        Task<PaymentResponseDto> CreateAsync(PaymentCreateDto createDto);
        Task<PaymentResponseDto> UpdateAsync(PaymentUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);

      
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentProcessDto processDto);
        Task<PaymentResponseDto> RefundPaymentAsync(int paymentId, decimal amount, string reason);
        Task<PaymentResponseDto> ConfirmPaymentAsync(int paymentId);

        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<decimal> GetTotalPaymentsAsync(int orderId);
        Task<decimal> GetTotalPaidAsync(int orderId);
        Task<PaymentSummaryDto> GetPaymentSummaryAsync(DateTime startDate, DateTime endDate);
    }

    public class PaymentProcessDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string TransactionId { get; set; }
    }

    public class PaymentUpdateDto
    {
        public int Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public string Notes { get; set; }
        public DateTime? ConfirmationDate { get; set; }
    }

    public class PaymentSummaryDto
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalRefunded { get; set; }
        public int TotalTransactions { get; set; }
        public Dictionary<string, decimal> PaymentsByMethod { get; set; }
    }
}