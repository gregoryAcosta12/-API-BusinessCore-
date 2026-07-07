using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponseDto<DashboardStatsDto>>> GetDashboardStats()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync();
            return Ok(new ApiResponseDto<DashboardStatsDto>(stats, "Estadísticas del dashboard obtenidas exitosamente"));
        }

        [HttpGet("sales-overview")]
        public async Task<ActionResult<ApiResponseDto<SalesOverviewDto>>> GetSalesOverview(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var overview = await _dashboardService.GetSalesOverviewAsync(startDate, endDate);
            return Ok(new ApiResponseDto<SalesOverviewDto>(overview, "Resumen de ventas obtenido exitosamente"));
        }

        [HttpGet("order-stats")]
        public async Task<ActionResult<ApiResponseDto<OrderStatsDto>>> GetOrderStats()
        {
            var stats = await _dashboardService.GetOrderStatsAsync();
            return Ok(new ApiResponseDto<OrderStatsDto>(stats, "Estadísticas de órdenes obtenidas exitosamente"));
        }

        [HttpGet("sales-report")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<SalesReportDto>>>> GetSalesReport(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _dashboardService.GetSalesReportAsync(startDate, endDate);
            return Ok(new ApiResponseDto<IEnumerable<SalesReportDto>>(report, "Reporte de ventas obtenido exitosamente"));
        }

        [HttpGet("monthly-sales/{year}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MonthlySalesDto>>>> GetMonthlySales(int year)
        {
            var sales = await _dashboardService.GetMonthlySalesAsync(year);
            return Ok(new ApiResponseDto<IEnumerable<MonthlySalesDto>>(sales, "Ventas mensuales obtenidas exitosamente"));
        }

        [HttpGet("top-products/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TopProductDto>>>> GetTopProducts(int count)
        {
            var products = await _dashboardService.GetTopProductsAsync(count);
            return Ok(new ApiResponseDto<IEnumerable<TopProductDto>>(products, "Productos top obtenidos exitosamente"));
        }

        [HttpGet("top-categories/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TopCategoryDto>>>> GetTopCategories(int count)
        {
            var categories = await _dashboardService.GetTopCategoriesAsync(count);
            return Ok(new ApiResponseDto<IEnumerable<TopCategoryDto>>(categories, "Categorías top obtenidas exitosamente"));
        }

        [HttpGet("top-customers/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TopCustomerDto>>>> GetTopCustomers(int count)
        {
            var customers = await _dashboardService.GetTopCustomersAsync(count);
            return Ok(new ApiResponseDto<IEnumerable<TopCustomerDto>>(customers, "Clientes top obtenidos exitosamente"));
        }

        [HttpGet("customer-stats")]
        public async Task<ActionResult<ApiResponseDto<CustomerStatsDto>>> GetCustomerStats()
        {
            var stats = await _dashboardService.GetCustomerStatsAsync();
            return Ok(new ApiResponseDto<CustomerStatsDto>(stats, "Estadísticas de clientes obtenidas exitosamente"));
        }

        [HttpGet("inventory-report")]
        public async Task<ActionResult<ApiResponseDto<InventoryReportDto>>> GetInventoryReport()
        {
            var report = await _dashboardService.GetInventoryReportAsync();
            return Ok(new ApiResponseDto<InventoryReportDto>(report, "Reporte de inventario obtenido exitosamente"));
        }

        [HttpGet("stock-alerts")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<StockAlertDto>>>> GetStockAlerts()
        {
            var alerts = await _dashboardService.GetStockAlertsAsync();
            return Ok(new ApiResponseDto<IEnumerable<StockAlertDto>>(alerts, "Alertas de stock obtenidas exitosamente"));
        }

        [HttpGet("financial-summary")]
        public async Task<ActionResult<ApiResponseDto<FinancialSummaryDto>>> GetFinancialSummary(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _dashboardService.GetFinancialSummaryAsync(startDate, endDate);
            return Ok(new ApiResponseDto<FinancialSummaryDto>(summary, "Resumen financiero obtenido exitosamente"));
        }

        [HttpGet("revenue-report")]
        public async Task<ActionResult<ApiResponseDto<RevenueReportDto>>> GetRevenueReport(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _dashboardService.GetRevenueReportAsync(startDate, endDate);
            return Ok(new ApiResponseDto<RevenueReportDto>(report, "Reporte de ingresos obtenido exitosamente"));
        }
    }
}