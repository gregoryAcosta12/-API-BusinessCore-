using System;
using System.Collections.Generic;

namespace BusinessCore.Domain.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public decimal? CostPrice { get; set; }
        public int Stock { get; set; }
        public int? MinStock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<ProductVariantAttribute> Attributes { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
    }
}