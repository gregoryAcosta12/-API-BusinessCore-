using BusinessCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Infrastructure.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pv => pv.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pv => pv.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(pv => pv.CostPrice)
                .HasPrecision(18, 2);

            builder.Property(pv => pv.Stock)
                .IsRequired();

            builder.Property(pv => pv.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(pv => pv.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices
            builder.HasIndex(pv => pv.Sku)
                .IsUnique()
                .HasDatabaseName("IX_ProductVariants_Sku");

            builder.HasIndex(pv => pv.ProductId)
                .HasDatabaseName("IX_ProductVariants_ProductId");

            // Relaciones
            builder.HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}