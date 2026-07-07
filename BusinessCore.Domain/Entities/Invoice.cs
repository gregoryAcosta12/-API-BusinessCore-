using System;

namespace BusinessCore.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceDue { get; set; }
        public InvoiceStatus Status { get; set; } // Paid, Pending, Overdue, Cancelled
        public string PdfUrl { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public virtual Order Order { get; set; }
        public virtual Customer Customer { get; set; }
    }
}