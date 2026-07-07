using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Invoice;
using BusinessCore.Application.DTOs.Invoices;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepository repository, IOrderRepository orderRepository, ICustomerRepository customerRepository, IMapper mapper)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<InvoiceResponseDto> GetByIdAsync(int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {id} no encontrada");

            return _mapper.Map<InvoiceResponseDto>(invoice);
        }

        public async Task<InvoiceResponseDto> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            var invoice = await _repository.GetByInvoiceNumberAsync(invoiceNumber);
            if (invoice == null)
                throw new NotFoundException($"Factura con número {invoiceNumber} no encontrada");

            return _mapper.Map<InvoiceResponseDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetOrderInvoiceAsync(int orderId)
        {
            var invoice = await _repository.GetByOrderIdAsync(orderId);
            if (invoice == null)
                return new List<InvoiceResponseDto>();

            return new List<InvoiceResponseDto> { _mapper.Map<InvoiceResponseDto>(invoice) };
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetCustomerInvoicesAsync(int customerId)
        {
            var invoices = await _repository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
        }

        public async Task<PagedResultDto<InvoiceResponseDto>> GetPagedAsync(InvoiceFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var invoices = await query;

            if (filter.OrderId.HasValue)
                invoices = invoices.Where(i => i.OrderId == filter.OrderId);

            if (filter.CustomerId.HasValue)
                invoices = invoices.Where(i => i.CustomerId == filter.CustomerId);

            if (!string.IsNullOrEmpty(filter.InvoiceNumber))
                invoices = invoices.Where(i => i.InvoiceNumber.Contains(filter.InvoiceNumber));

            if (filter.Status.HasValue)
                invoices = invoices.Where(i => i.Status == filter.Status);

            if (filter.StartDate.HasValue)
                invoices = invoices.Where(i => i.InvoiceDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                invoices = invoices.Where(i => i.InvoiceDate <= filter.EndDate);

            var totalCount = invoices.Count();
            var items = invoices
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<InvoiceResponseDto>
            {
                Items = _mapper.Map<IEnumerable<InvoiceResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<InvoiceResponseDto> CreateAsync(InvoiceCreateDto createDto)
        {
            var order = await _orderRepository.GetByIdAsync(createDto.OrderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {createDto.OrderId} no encontrada");

            var invoice = _mapper.Map<Invoice>(createDto);
            invoice.InvoiceNumber = GenerateInvoiceNumber();
            invoice.InvoiceDate = DateTime.UtcNow;
            invoice.Status = InvoiceStatus.Draft;
            invoice.Subtotal = order.Subtotal;
            invoice.TaxAmount = order.TaxAmount;
            invoice.TotalAmount = order.TotalAmount;
            invoice.AmountPaid = 0;
            invoice.BalanceDue = order.TotalAmount;

            var created = await _repository.CreateAsync(invoice);
            return _mapper.Map<InvoiceResponseDto>(created);
        }

        public async Task<InvoiceResponseDto> UpdateAsync(InvoiceUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Factura con ID {updateDto.Id} no encontrada");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<InvoiceResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Factura con ID {id} no encontrada");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<InvoiceResponseDto> SendInvoiceAsync(int invoiceId)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {invoiceId} no encontrada");

            invoice.Status = InvoiceStatus.Sent;
            invoice.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(invoice);
            return _mapper.Map<InvoiceResponseDto>(updated);
        }

        public async Task<InvoiceResponseDto> MarkAsPaidAsync(int invoiceId, decimal amount)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {invoiceId} no encontrada");

            invoice.AmountPaid += amount;
            invoice.BalanceDue = invoice.TotalAmount - invoice.AmountPaid;

            if (invoice.BalanceDue <= 0)
                invoice.Status = InvoiceStatus.Paid;
            else
                invoice.Status = InvoiceStatus.PartiallyPaid;

            invoice.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(invoice);
            return _mapper.Map<InvoiceResponseDto>(updated);
        }

        public async Task<InvoiceResponseDto> MarkAsOverdueAsync(int invoiceId)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {invoiceId} no encontrada");

            if (invoice.DueDate < DateTime.UtcNow && invoice.Status != InvoiceStatus.Paid)
            {
                invoice.Status = InvoiceStatus.Overdue;
                invoice.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(invoice);
            }

            return _mapper.Map<InvoiceResponseDto>(invoice);
        }

        public async Task<InvoiceResponseDto> CancelInvoiceAsync(int invoiceId, string reason)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {invoiceId} no encontrada");

            if (invoice.Status == InvoiceStatus.Paid)
                throw new BusinessException("No se puede cancelar una factura ya pagada");

            invoice.Status = InvoiceStatus.Cancelled;
            invoice.Notes = $"Cancelado: {reason}";
            invoice.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(invoice);
            return _mapper.Map<InvoiceResponseDto>(updated);
        }

        public async Task<InvoiceResponseDto> GenerateInvoiceFromOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {orderId} no encontrada");

            var existingInvoice = await _repository.GetByOrderIdAsync(orderId);
            if (existingInvoice != null)
                throw new ConflictException($"La orden {orderId} ya tiene una factura asociada");

            var invoice = new Invoice
            {
                OrderId = orderId,
                CustomerId = order.CustomerId,
                InvoiceNumber = GenerateInvoiceNumber(),
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Subtotal = order.Subtotal,
                TaxAmount = order.TaxAmount,
                TotalAmount = order.TotalAmount,
                AmountPaid = 0,
                BalanceDue = order.TotalAmount,
                Status = InvoiceStatus.Draft,
                Notes = $"Factura generada desde la orden {order.OrderNumber}"
            };

            var created = await _repository.CreateAsync(invoice);
            return _mapper.Map<InvoiceResponseDto>(created);
        }

        public async Task<string> GenerateInvoicePdfAsync(int invoiceId)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new NotFoundException($"Factura con ID {invoiceId} no encontrada");

            // Aquí iría la generación del PDF
            return $"https://example.com/invoices/{invoice.InvoiceNumber}.pdf";
        }

        public async Task<InvoiceStatisticsDto> GetInvoiceStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var invoices = await _repository.GetByDateRangeAsync(startDate, endDate);
            var invoiceList = invoices.ToList();

            return new InvoiceStatisticsDto
            {
                TotalInvoices = invoiceList.Count,
                TotalAmount = invoiceList.Sum(i => i.TotalAmount),
                TotalPaid = invoiceList.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.TotalAmount),
                TotalOutstanding = invoiceList.Where(i => i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Cancelled).Sum(i => i.BalanceDue),
                PaidCount = invoiceList.Count(i => i.Status == InvoiceStatus.Paid),
                OverdueCount = invoiceList.Count(i => i.Status == InvoiceStatus.Overdue),
                PendingCount = invoiceList.Count(i => i.Status == InvoiceStatus.Draft || i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.PartiallyPaid),
                CancelledCount = invoiceList.Count(i => i.Status == InvoiceStatus.Cancelled)
            };
        }

        public async Task<decimal> GetTotalOutstandingAsync()
        {
            return await _repository.GetTotalOutstandingAsync();
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetOverdueInvoicesAsync()
        {
            var invoices = await _repository.GetOverdueInvoicesAsync();
            return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
        }

        private string GenerateInvoiceNumber()
        {
            var year = DateTime.UtcNow.ToString("yyyy");
            var month = DateTime.UtcNow.ToString("MM");
            var random = new Random().Next(1000, 9999);
            return $"INV-{year}{month}-{random}";
        }
    }
}