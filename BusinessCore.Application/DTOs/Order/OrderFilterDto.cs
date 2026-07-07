using System;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Order
{
    public class OrderFilterDto
    {
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }
        public string OrderNumber { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}