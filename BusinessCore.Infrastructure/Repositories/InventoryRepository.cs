using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryMovement> GetByIdAsync(int id)
        {
            return await _context.InventoryMovements
                .Include(im => im.Product)
                .Include(im => im.ProductVariant)
                .Include(im => im.SourceWarehouse)
                .Include(im => im.TargetWarehouse)
                .FirstOrDefaultAsync(im => im.Id == id);
        }

        public async Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId)
        {
            return await _context.InventoryMovements
                .Include(im => im.SourceWarehouse)
                .Include(im => im.TargetWarehouse)
                .Where(im => im.ProductId == productId)
                .OrderByDescending(im => im.MovementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryMovement>> GetMovementsByProductVariantIdAsync(int variantId)
        {
            return await _context.InventoryMovements
                .Include(im => im.SourceWarehouse)
                .Include(im => im.TargetWarehouse)
                .Where(im => im.ProductVariantId == variantId)
                .OrderByDescending(im => im.MovementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryMovement>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.InventoryMovements
                .Include(im => im.Product)
                .Include(im => im.SourceWarehouse)
                .Include(im => im.TargetWarehouse)
                .Where(im => im.MovementDate >= startDate && im.MovementDate <= endDate)
                .OrderByDescending(im => im.MovementDate)
                .ToListAsync();
        }

        public async Task<InventoryMovement> CreateMovementAsync(InventoryMovement movement)
        {
            movement.CreatedAt = DateTime.UtcNow;
            movement.MovementDate = DateTime.UtcNow;
            _context.InventoryMovements.Add(movement);
            await _context.SaveChangesAsync();
            return movement;
        }

        public async Task<IEnumerable<InventoryMovement>> GetLastMovementsAsync(int productId, int count)
        {
            return await _context.InventoryMovements
                .Where(im => im.ProductId == productId)
                .OrderByDescending(im => im.MovementDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetCurrentStockAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.Stock ?? 0;
        }

        public async Task<decimal> GetCurrentStockAsync(int productId, int? warehouseId)
        {
            if (!warehouseId.HasValue)
                return await GetCurrentStockAsync(productId);

            var movements = await _context.InventoryMovements
                .Where(im => im.ProductId == productId &&
                             (im.SourceWarehouseId == warehouseId || im.TargetWarehouseId == warehouseId))
                .ToListAsync();

            decimal stock = 0;
            foreach (var movement in movements)
            {
                if (movement.SourceWarehouseId == warehouseId)
                    stock -= movement.Quantity;
                else if (movement.TargetWarehouseId == warehouseId)
                    stock += movement.Quantity;
            }

            return stock;
        }

        public async Task<IEnumerable<InventoryMovement>> GetWarehouseMovementsAsync(int warehouseId)
        {
            return await _context.InventoryMovements
                .Include(im => im.Product)
                .Include(im => im.ProductVariant)
                .Where(im => im.SourceWarehouseId == warehouseId || im.TargetWarehouseId == warehouseId)
                .OrderByDescending(im => im.MovementDate)
                .ToListAsync();
        }

        public async Task<bool> TransferStockAsync(int productId, int sourceWarehouseId, int targetWarehouseId, int quantity)
        {
            // Verificar stock en origen
            var sourceStock = await GetCurrentStockAsync(productId, sourceWarehouseId);
            if (sourceStock < quantity)
                return false;

            // Crear movimiento de salida
            var outMovement = new InventoryMovement
            {
                ProductId = productId,
                Type = MovementType.Transfer,
                Quantity = quantity,
                UnitCost = 0,
                SourceWarehouseId = sourceWarehouseId,
                TargetWarehouseId = targetWarehouseId,
                Reference = $"TRANSFER-{DateTime.UtcNow:yyyyMMddHHmmss}",
                Notes = $"Transferencia de {quantity} unidades de {sourceWarehouseId} a {targetWarehouseId}",
                CreatedAt = DateTime.UtcNow,
                MovementDate = DateTime.UtcNow
            };

            _context.InventoryMovements.Add(outMovement);
            await _context.SaveChangesAsync();

            // Actualizar stock del producto (si es el stock global)
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}