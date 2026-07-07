using System;

namespace BusinessCore.Domain.Entities
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public MovementType Type { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime MovementDate { get; set; }
        public string Reference { get; set; } 
        public string Notes { get; set; }
        public int? SourceWarehouseId { get; set; }
        public int? TargetWarehouseId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual Warehouse SourceWarehouse { get; set; }
        public virtual Warehouse TargetWarehouse { get; set; }
    }
}