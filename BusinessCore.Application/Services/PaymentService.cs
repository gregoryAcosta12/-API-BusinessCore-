using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Payment;
using BusinessCore.Application.DTOs.Payments;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository repository, IOrderRepository orderRepository, IMapper mapper)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponseDto> GetByIdAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException($"Pago con ID {id} no encontrado");

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<PaymentResponseDto> GetByTransactionIdAsync(string transactionId)
        {
            var payment = await _repository.GetByTransactionIdAsync(transactionId);
            if (payment == null)
                throw new NotFoundException($"Pago con TransactionId {transactionId} no encontrado");

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetOrderPaymentsAsync(int orderId)
        {
            var payments = await _repository.GetOrderPaymentsAsync(orderId);
            return _mapper.Map<IEnumerable<PaymentResponseDto>>(payments);
        }

        public async Task<PagedResultDto<PaymentResponseDto>> GetPagedAsync(PaymentFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var payments = await query;

            if (filter.OrderId.HasValue)
                payments = payments.Where(p => p.OrderId == filter.OrderId);

            if (!string.IsNullOrEmpty(filter.PaymentMethod))
                payments = payments.Where(p => p.PaymentMethod == filter.PaymentMethod);

            if (filter.Status.HasValue)
                payments = payments.Where(p => p.Status == filter.Status);

            if (filter.StartDate.HasValue)
                payments = payments.Where(p => p.PaymentDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                payments = payments.Where(p => p.PaymentDate <= filter.EndDate);

            var totalCount = payments.Count();
            var items = payments
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<PaymentResponseDto>
            {
                Items = _mapper.Map<IEnumerable<PaymentResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<PaymentResponseDto> CreateAsync(PaymentCreateDto createDto)
        {
            var order = await _orderRepository.GetByIdAsync(createDto.OrderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {createDto.OrderId} no encontrada");

            var payment = _mapper.Map<Payment>(createDto);
            payment.Status = PaymentStatus.Pending;
            payment.PaymentDate = DateTime.UtcNow;

            var created = await _repository.CreateAsync(payment);
            return _mapper.Map<PaymentResponseDto>(created);
        }

        public async Task<PaymentResponseDto> UpdateAsync(PaymentUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Pago con ID {updateDto.Id} no encontrado");

            _mapper.Map(updateDto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<PaymentResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Pago con ID {id} no encontrado");

            return await _repository.DeleteAsync(id);
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentProcessDto processDto)
        {
            var order = await _orderRepository.GetByIdAsync(processDto.OrderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {processDto.OrderId} no encontrada");

            if (order.Status == OrderStatus.Cancelled)
                throw new BusinessException("No se puede procesar el pago de una orden cancelada");

            // Simular procesamiento de pago
            var payment = new Payment
            {
                OrderId = processDto.OrderId,
                PaymentMethod = processDto.PaymentMethod,
                Amount = processDto.Amount,
                TransactionId = processDto.TransactionId ?? $"TXN-{Guid.NewGuid():N}",
                Status = PaymentStatus.Paid,
                PaymentDate = DateTime.UtcNow,
                ConfirmationDate = DateTime.UtcNow,
                Notes = "Pago procesado exitosamente"
            };

            var created = await _repository.CreateAsync(payment);
            return _mapper.Map<PaymentResponseDto>(created);
        }

        public async Task<PaymentResponseDto> RefundPaymentAsync(int paymentId, decimal amount, string reason)
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new NotFoundException($"Pago con ID {paymentId} no encontrado");

            if (payment.Status != PaymentStatus.Paid)
                throw new BusinessException("Solo se pueden reembolsar pagos ya confirmados");

            if (amount > payment.Amount)
                throw new BusinessException("El monto a reembolsar no puede exceder el monto del pago");

            payment.Status = PaymentStatus.Refunded;
            payment.Notes = $"Reembolso: {reason}";
            await _repository.UpdateAsync(payment);

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<PaymentResponseDto> ConfirmPaymentAsync(int paymentId)
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new NotFoundException($"Pago con ID {paymentId} no encontrado");

            payment.Status = PaymentStatus.Paid;
            payment.ConfirmationDate = DateTime.UtcNow;
            await _repository.UpdateAsync(payment);

            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            var payments = await _repository.GetPaymentsByStatusAsync(status);
            return _mapper.Map<IEnumerable<PaymentResponseDto>>(payments);
        }

        public async Task<decimal> GetTotalPaymentsAsync(int orderId)
        {
            return await _repository.GetTotalPaymentsAsync(orderId);
        }

        public async Task<decimal> GetTotalPaidAsync(int orderId)
        {
            return await _repository.GetTotalPaymentsAsync(orderId);
        }

        public async Task<PaymentSummaryDto> GetPaymentSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _repository.GetPaymentsByDateRangeAsync(startDate, endDate);
            var paymentList = payments.ToList();

            return new PaymentSummaryDto
            {
                TotalAmount = paymentList.Sum(p => p.Amount),
                TotalPaid = paymentList.Where(p => p.Status == PaymentStatus.Paid).Sum(p => p.Amount),
                TotalPending = paymentList.Where(p => p.Status == PaymentStatus.Pending).Sum(p => p.Amount),
                TotalRefunded = paymentList.Where(p => p.Status == PaymentStatus.Refunded).Sum(p => p.Amount),
                TotalTransactions = paymentList.Count,
                PaymentsByMethod = paymentList.GroupBy(p => p.PaymentMethod).ToDictionary(g => g.Key, g => g.Sum(p => p.Amount))
            };
        }
    }
}