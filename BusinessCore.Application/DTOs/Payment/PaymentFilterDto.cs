using BusinessCore.Domain.Enums;

using Syusing System;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Payment
{
    public class PaymentFilterDto
    {
        public int? OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}