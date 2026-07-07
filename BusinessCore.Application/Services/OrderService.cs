using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Order;
using BusinessCore.Application.DTOs.Orders;
using BusinessCore.Application.DTOs.Payments;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IProductRepository productRepository, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                throw new NotFoundException($"Orden con ID {id} no encontrada");

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _repository.GetByOrderNumberAsync(orderNumber);
            if (order == null)
                throw new NotFoundException($"Orden con número {orderNumber} no encontrada");

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<PagedResultDto<OrderResponseDto>> GetPagedAsync(OrderFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var orders = await query;

            if (filter.UserId.HasValue)
                orders = orders.Where(o => o.UserId == filter.UserId);

            if (filter.CustomerId.HasValue)
                orders = orders.Where(o => o.CustomerId == filter.CustomerId);

            if (filter.Status.HasValue)
                orders = orders.Where(o => o.Status == filter.Status);

            if (filter.PaymentStatus.HasValue)
                orders = orders.Where(o => o.PaymentStatus == filter.PaymentStatus);

            if (filter.StartDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= filter.EndDate);

            if (!string.IsNullOrEmpty(filter.OrderNumber))
                orders = orders.Where(o => o.OrderNumber.Contains(filter.OrderNumber));

            var totalCount = orders.Count();
            var items = orders
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<OrderResponseDto>
            {
                Items = _mapper.Map<IEnumerable<OrderResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<OrderResponseDto> CreateAsync(OrderCreateDto createDto)
        {
            if (createDto.Items == null || !createDto.Items.Any())
                throw new BadRequestException("La orden debe tener al menos un item");

            var order = _mapper.Map<Order>(createDto);
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.PaymentStatus = PaymentStatus.Pending;

            decimal subtotal = 0;
            foreach (var itemDto in createDto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {itemDto.ProductId} no encontrado");

                if (product.Stock < itemDto.Quantity)
                    throw new BusinessException($"Stock insuficiente para el producto {product.Name}");

                var orderItem = _mapper.Map<OrderItem>(itemDto);
                orderItem.ProductName = product.Name;
                orderItem.ProductSku = product.Sku;
                orderItem.UnitPrice = itemDto.UnitPrice;
                orderItem.TotalPrice = itemDto.UnitPrice * itemDto.Quantity - itemDto.Discount;
                orderItem.CreatedAt = DateTime.UtcNow;

                subtotal += orderItem.TotalPrice;
                order.OrderItems.Add(orderItem);

                // Actualizar stock
                await _productRepository.UpdateStockAsync(product.Id, product.Stock - itemDto.Quantity);
            }

            order.Subtotal = subtotal;
            order.TaxAmount = subtotal * 0.16m; // 16% IVA
            order.ShippingAmount = 0;
            order.DiscountAmount = 0;
            order.TotalAmount = order.Subtotal + order.TaxAmount + order.ShippingAmount - order.DiscountAmount;

            var created = await _repository.CreateAsync(order);
            return _mapper.Map<OrderResponseDto>(created);
        }

        public async Task<OrderResponseDto> UpdateAsync(OrderUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Orden con ID {updateDto.Id} no encontrada");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<OrderResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Orden con ID {id} no encontrada");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _repository.GetUserOrdersAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _repository.GetOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _repository.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<OrderResponseDto> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _repository.UpdateStatusAsync(orderId, status);
            if (order == null)
                throw new NotFoundException($"Orden con ID {orderId} no encontrada");

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto> CancelOrderAsync(int orderId, string reason)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {orderId} no encontrada");

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Shipped)
                throw new BusinessException("No se puede cancelar una orden ya enviada o entregada");

            order.Status = OrderStatus.Cancelled;
            order.Notes = $"Cancelado: {reason}";
            order.UpdatedAt = DateTime.UtcNow;

            // Restaurar stock
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                    await _productRepository.UpdateStockAsync(product.Id, product.Stock + item.Quantity);
            }

            var updated = await _repository.UpdateAsync(order);
            return _mapper.Map<OrderResponseDto>(updated);
        }

        public async Task<bool> UpdateShippingInfoAsync(int orderId, string trackingNumber, string shippingMethod)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException($"Orden con ID {orderId} no encontrada");

            order.TrackingNumber = trackingNumber;
            order.ShippingMethod = shippingMethod;
            order.ShippedDate = DateTime.UtcNow;
            order.Status = OrderStatus.Shipped;
            order.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<int> GetOrderCountByStatusAsync(OrderStatus status)
        {
            return await _repository.GetOrderCountByStatusAsync(status);
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            return await _repository.GetTotalSalesAsync(startDate, endDate);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetRecentOrdersAsync(int count)
        {
            var orders = await _repository.GetRecentOrdersAsync(count);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _repository.GetOrdersByDateRangeAsync(startDate, endDate);
            var orderList = orders.ToList();

            return new OrderStatisticsDto
            {
                TotalOrders = orderList.Count,
                TotalRevenue = orderList.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                AverageOrderValue = orderList.Any() ? orderList.Average(o => o.TotalAmount) : 0,
                PendingOrders = orderList.Count(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = orderList.Count(o => o.Status == OrderStatus.Processing),
                ShippedOrders = orderList.Count(o => o.Status == OrderStatus.Shipped),
                DeliveredOrders = orderList.Count(o => o.Status == OrderStatus.Delivered),
                CancelledOrders = orderList.Count(o => o.Status == OrderStatus.Cancelled)
            };
        }
    }
}