using BusinessCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Infrastructure.Configurations
{
    public class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovement>
    {
        public void Configure(EntityTypeBuilder<InventoryMovement> builder)
        {
            builder.ToTable("InventoryMovements");

            builder.HasKey(im => im.Id);

            builder.Property(im => im.Quantity)
                .IsRequired();

            builder.Property(im => im.UnitCost)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(im => im.Reference)
                .HasMaxLength(100);

            builder.Property(im => im.Notes)
                .HasMaxLength(500);

            builder.Property(im => im.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Índices
            builder.HasIndex(im => im.ProductId)
                .HasDatabaseName("IX_InventoryMovements_ProductId");

            builder.HasIndex(im => im.ProductVariantId)
                .HasDatabaseName("IX_InventoryMovements_ProductVariantId");

            builder.HasIndex(im => im.MovementDate)
                .HasDatabaseName("IX_InventoryMovements_MovementDate");

            builder.HasIndex(im => im.Type)
                .HasDatabaseName("IX_InventoryMovements_Type");

            builder.HasIndex(im => im.SourceWarehouseId)
                .HasDatabaseName("IX_InventoryMovements_SourceWarehouseId");

            builder.HasIndex(im => im.TargetWarehouseId)
                .HasDatabaseName("IX_InventoryMovements_TargetWarehouseId");

            // Relaciones
            builder.HasOne(im => im.Product)
                .WithMany(p => p.InventoryMovements)
                .HasForeignKey(im => im.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(im => im.ProductVariant)
                .WithMany(pv => pv.InventoryMovements)
                .HasForeignKey(im => im.ProductVariantId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(im => im.SourceWarehouse)
                .WithMany(w => w.SourceMovements)
                .HasForeignKey(im => im.SourceWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(im => im.TargetWarehouse)
                .WithMany(w => w.TargetMovements)
                .HasForeignKey(im => im.TargetWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}