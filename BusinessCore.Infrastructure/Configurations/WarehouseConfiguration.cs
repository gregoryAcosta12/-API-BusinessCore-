using BusinessCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Infrastructure.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(w => w.Address)
                .HasMaxLength(200);

            builder.Property(w => w.City)
                .HasMaxLength(100);

            builder.Property(w => w.State)
                .HasMaxLength(100);

            builder.Property(w => w.Country)
                .HasMaxLength(100);

            builder.Property(w => w.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(w => w.Email)
                .HasMaxLength(150);

            builder.Property(w => w.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(w => w.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices
            builder.HasIndex(w => w.Code)
                .IsUnique()
                .HasDatabaseName("IX_Warehouses_Code");
        }
    }
}