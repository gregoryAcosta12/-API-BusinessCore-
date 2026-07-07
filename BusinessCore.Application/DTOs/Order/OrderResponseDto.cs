using System;
using System.Collections.Generic;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName => PaymentStatus.ToString();
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; }
        public Address.AddressResponseDto ShippingAddress { get; set; }
        public List<Payment.PaymentResponseDto> Payments { get; set; }
    }
}
}