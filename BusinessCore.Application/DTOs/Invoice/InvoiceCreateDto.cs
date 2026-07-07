using System;

namespace BusinessCore.Application.DTOs.Invoice
{
    public class InvoiceCreateDto
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
    }
}