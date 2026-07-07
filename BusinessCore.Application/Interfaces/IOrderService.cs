using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Order;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.Interfaces
{
 
    public interface IOrderService
    {
        
        Task<OrderResponseDto> GetByIdAsync(int id);
        Task<OrderResponseDto> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<PagedResultDto<OrderResponseDto>> GetPagedAsync(OrderFilterDto filter);
        Task<OrderResponseDto> CreateAsync(OrderCreateDto createDto);
        Task<OrderResponseDto> UpdateAsync(OrderUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Consultas específicas
        Task<IEnumerable<OrderResponseDto>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<OrderResponseDto>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

      
        Task<OrderResponseDto> UpdateStatusAsync(int orderId, OrderStatus status);
        Task<OrderResponseDto> CancelOrderAsync(int orderId, string reason);
        Task<OrderResponseDto> ProcessPaymentAsync(int orderId, Payment.PaymentCreateDto paymentDto);
        Task<bool> UpdateShippingInfoAsync(int orderId, string trackingNumber, string shippingMethod);

        
        Task<int> GetOrderCountByStatusAsync(OrderStatus status);
        Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderResponseDto>> GetRecentOrdersAsync(int count);
        Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime startDate, DateTime endDate);
    }

    public class OrderStatisticsDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
    }
}