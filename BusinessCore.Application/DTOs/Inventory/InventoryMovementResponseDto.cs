using System;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Inventory
{
    public class InventoryMovementResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ProductVariantId { get; set; }
        public string VariantName { get; set; }
        public MovementType Type { get; set; }
        public string TypeName => Type.ToString();
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime MovementDate { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public int? SourceWarehouseId { get; set; }
        public string SourceWarehouseName { get; set; }
        public int? TargetWarehouseId { get; set; }
        public string TargetWarehouseName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}