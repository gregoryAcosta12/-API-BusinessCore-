using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Dashboard;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Dashboard
    /// Define las operaciones para obtener estadísticas y reportes
    /// </summary>
    public interface IDashboardService
    {
        // Dashboard principal
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<SalesOverviewDto> GetSalesOverviewAsync(DateTime startDate, DateTime endDate);
        Task<OrderStatsDto> GetOrderStatsAsync();

        // Reportes de ventas
        Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MonthlySalesDto>> GetMonthlySalesAsync(int year);
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int count);
        Task<IEnumerable<TopCategoryDto>> GetTopCategoriesAsync(int count);

        // Reportes de clientes
        Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int count);
        Task<CustomerStatsDto> GetCustomerStatsAsync();

        // Reportes de inventario
        Task<InventoryReportDto> GetInventoryReportAsync();
        Task<IEnumerable<StockAlertDto>> GetStockAlertsAsync();

        // Reportes financieros
        Task<FinancialSummaryDto> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate);
        Task<RevenueReportDto> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
    }

    public class DashboardStatsDto
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockProducts { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int MonthlyOrders { get; set; }
        public double RevenueGrowth { get; set; }
    }

    public class SalesOverviewDto
    {
        public decimal TotalSales { get; set; }
        public decimal TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal ConversionRate { get; set; }
        public Dictionary<DateTime, decimal> DailySales { get; set; }
        public Dictionary<string, decimal> SalesByCategory { get; set; }
    }

    public class OrderStatsDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueByStatus { get; set; }
    }

    public class SalesReportDto
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AverageAmount { get; set; }
        public int CustomerCount { get; set; }
    }

    public class MonthlySalesDto
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Growth { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class TopCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalProducts { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class CustomerStatsDto
    {
        public int TotalCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal AverageCustomerLifetimeValue { get; set; }
        public double CustomerRetentionRate { get; set; }
    }

    public class InventoryReportDto
    {
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public Dictionary<string, int> StockByCategory { get; set; }
        public decimal InventoryTurnover { get; set; }
    }

    public class StockAlertDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public string Severity => CurrentStock <= 0 ? "CRITICAL" : "WARNING";
    }

    public class FinancialSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public decimal TotalExpenses { get; set; }
        public Dictionary<string, decimal> RevenueBreakdown { get; set; }
    }

    public class RevenueReportDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public Dictionary<string, decimal> DailyRevenue { get; set; }
        public Dictionary<string, decimal> RevenueByProduct { get; set; }
    }
}