using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Inventory
{
    public class InventoryMovementCreateDto
    {
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public MovementType Type { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public int? SourceWarehouseId { get; set; }
        public int? TargetWarehouseId { get; set; }
    }
}