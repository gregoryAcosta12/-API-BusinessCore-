using BusinessCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Infrastructure.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Subtotal)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.TaxAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.AmountPaid)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.BalanceDue)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.PdfUrl)
                .HasMaxLength(500);

            builder.Property(i => i.Notes)
                .HasMaxLength(500);

            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Índices
            builder.HasIndex(i => i.InvoiceNumber)
                .IsUnique()
                .HasDatabaseName("IX_Invoices_InvoiceNumber");

            builder.HasIndex(i => i.OrderId)
                .HasDatabaseName("IX_Invoices_OrderId");

            builder.HasIndex(i => i.CustomerId)
                .HasDatabaseName("IX_Invoices_CustomerId");

            builder.HasIndex(i => i.Status)
                .HasDatabaseName("IX_Invoices_Status");

            // Relaciones
            builder.HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}