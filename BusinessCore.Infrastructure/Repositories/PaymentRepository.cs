using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Payment>> GetOrderPaymentsAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            payment.CreatedAt = DateTime.UtcNow;
            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = PaymentStatus.Pending;

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Actualizar estado del pedido
            await UpdateOrderPaymentStatus(payment.OrderId);

            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Actualizar estado del pedido
            await UpdateOrderPaymentStatus(payment.OrderId);

            return payment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            // Actualizar estado del pedido
            await UpdateOrderPaymentStatus(payment.OrderId);

            return true;
        }

        public async Task<decimal> GetTotalPaymentsAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId && p.Status == PaymentStatus.Paid)
                .SumAsync(p => p.Amount);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        private async Task UpdateOrderPaymentStatus(int orderId)
        {
            var payments = await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return;

            var totalPaid = payments
                .Where(p => p.Status == PaymentStatus.Paid)
                .Sum(p => p.Amount);

            var totalPending = payments
                .Where(p => p.Status == PaymentStatus.Pending)
                .Sum(p => p.Amount);

            if (totalPaid >= order.TotalAmount)
            {
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid > 0 && totalPaid < order.TotalAmount)
            {
                order.PaymentStatus = PaymentStatus.PartiallyRefunded;
            }
            else if (totalPending > 0)
            {
                order.PaymentStatus = PaymentStatus.Pending;
            }
            else
            {
                order.PaymentStatus = PaymentStatus.Failed;
            }

            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}