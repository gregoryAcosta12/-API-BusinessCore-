using System;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Inventory
{
    public class InventoryFilterDto
    {
        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public int? WarehouseId { get; set; }
        public MovementType? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Reference { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}