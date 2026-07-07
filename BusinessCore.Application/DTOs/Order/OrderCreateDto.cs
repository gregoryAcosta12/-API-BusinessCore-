using System;
using System.Collections.Generic;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Order
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public int AddressId { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Notes { get; set; }
        public string ShippingMethod { get; set; }
        public string Currency { get; set; } = "USD";

        public List<OrderItemCreateDto> Items { get; set; }
        public Payment.PaymentCreateDto Payment { get; set; }
    }
}