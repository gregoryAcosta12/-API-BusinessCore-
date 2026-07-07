using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order> UpdateStatusAsync(int orderId, OrderStatus status);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetOrderCountByStatusAsync(OrderStatus status);
        Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Order>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    }
}