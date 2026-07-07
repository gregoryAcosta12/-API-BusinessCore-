using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Invoice;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.Interfaces
{
   
    public interface IInvoiceService
    {
        
        Task<InvoiceResponseDto> GetByIdAsync(int id);
        Task<InvoiceResponseDto> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<IEnumerable<InvoiceResponseDto>> GetOrderInvoiceAsync(int orderId);
        Task<IEnumerable<InvoiceResponseDto>> GetCustomerInvoicesAsync(int customerId);
        Task<PagedResultDto<InvoiceResponseDto>> GetPagedAsync(InvoiceFilterDto filter);
        Task<InvoiceResponseDto> CreateAsync(InvoiceCreateDto createDto);
        Task<InvoiceResponseDto> UpdateAsync(InvoiceUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        
        Task<InvoiceResponseDto> SendInvoiceAsync(int invoiceId);
        Task<InvoiceResponseDto> MarkAsPaidAsync(int invoiceId, decimal amount);
        Task<InvoiceResponseDto> MarkAsOverdueAsync(int invoiceId);
        Task<InvoiceResponseDto> CancelInvoiceAsync(int invoiceId, string reason);

       
        Task<InvoiceResponseDto> GenerateInvoiceFromOrderAsync(int orderId);
        Task<string> GenerateInvoicePdfAsync(int invoiceId);

       
        Task<InvoiceStatisticsDto> GetInvoiceStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalOutstandingAsync();
        Task<IEnumerable<InvoiceResponseDto>> GetOverdueInvoicesAsync();
    }

    public class InvoiceUpdateDto
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public string Notes { get; set; }
        public decimal? AmountPaid { get; set; }
    }

    public class InvoiceStatisticsDto
    {
        public int TotalInvoices { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int PaidCount { get; set; }
        public int OverdueCount { get; set; }
        public int PendingCount { get; set; }
        public int CancelledCount { get; set; }
    }
}