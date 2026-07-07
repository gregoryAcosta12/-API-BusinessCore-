using System;
using System.Collections.Generic;

namespace BusinessCore.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; } 
        public decimal Price { get; set; }
        public decimal? CostPrice { get; set; } 
        public int Stock { get; set; }
        public int? MinStock { get; set; } 
        public int? MaxStock { get; set; } 
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

      
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<ProductVariant> Variants { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }
        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}