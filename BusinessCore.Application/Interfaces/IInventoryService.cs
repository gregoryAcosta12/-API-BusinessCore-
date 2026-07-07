using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Inventory;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de inventario
    /// Define las operaciones de negocio para la gestión de inventario
    /// </summary>
    public interface IInventoryService
    {
        // CRUD Básico
        Task<InventoryMovementResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<InventoryMovementResponseDto>> GetMovementsByProductIdAsync(int productId);
        Task<IEnumerable<InventoryMovementResponseDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<PagedResultDto<InventoryMovementResponseDto>> GetPagedAsync(InventoryFilterDto filter);
        Task<InventoryMovementResponseDto> CreateMovementAsync(InventoryMovementCreateDto createDto);
        Task<bool> DeleteMovementAsync(int id);

        // Operaciones de inventario
        Task<bool> AdjustStockAsync(int productId, int quantity, string reason);
        Task<bool> TransferStockAsync(int productId, int sourceWarehouseId, int targetWarehouseId, int quantity);
        Task<bool> ReceiveStockAsync(int productId, int quantity, decimal unitCost, string reference);
        Task<bool> ReleaseStockAsync(int productId, int quantity, string reference);

        // Consultas de stock
        Task<int> GetCurrentStockAsync(int productId);
        Task<int> GetCurrentStockAsync(int productId, int warehouseId);
        Task<IEnumerable<ProductStockDto>> GetLowStockProductsAsync(int threshold);
        Task<IEnumerable<ProductStockDto>> GetProductsWithStockAsync();

        // Estadísticas
        Task<InventoryStatisticsDto> GetInventoryStatisticsAsync();
        Task<IEnumerable<WarehouseStockDto>> GetWarehouseStockSummaryAsync();
        Task<decimal> GetTotalInventoryValueAsync();
    }

    public class ProductStockDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int CurrentStock { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public bool IsLowStock => MinStock.HasValue && CurrentStock <= MinStock.Value;
        public bool IsOutOfStock => CurrentStock <= 0;
    }

    public class WarehouseStockDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalValue { get; set; }
        public Dictionary<int, int> ProductCounts { get; set; }
    }

    public class InventoryStatisticsDto
    {
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int TotalMovements { get; set; }
        public int MonthlyMovements { get; set; }
        public Dictionary<MovementType, int> MovementsByType { get; set; }
    }
}