using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetOrderPaymentsAsync(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int id);
        Task<decimal> GetTotalPaymentsAsync(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}