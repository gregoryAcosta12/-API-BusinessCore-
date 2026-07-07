using BusinessCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<InventoryMovement> GetByIdAsync(int id);
        Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId);
        Task<IEnumerable<InventoryMovement>> GetMovementsByProductVariantIdAsync(int variantId);
        Task<IEnumerable<InventoryMovement>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<InventoryMovement> CreateMovementAsync(InventoryMovement movement);
        Task<IEnumerable<InventoryMovement>> GetLastMovementsAsync(int productId, int count);
        Task<decimal> GetCurrentStockAsync(int productId);
        Task<decimal> GetCurrentStockAsync(int productId, int? warehouseId);
        Task<IEnumerable<InventoryMovement>> GetWarehouseMovementsAsync(int warehouseId);
        Task<bool> TransferStockAsync(int productId, int sourceWarehouseId, int targetWarehouseId, int quantity);
    }
}