using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Order;
using BusinessCore.Application.DTOs.Orders;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<OrderResponseDto>>(orders, "Órdenes obtenidas exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<OrderResponseDto>(order, "Orden obtenida exitosamente"));
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetByOrderNumber(string orderNumber)
        {
            var order = await _orderService.GetByOrderNumberAsync(orderNumber);
            return Ok(new ApiResponseDto<OrderResponseDto>(order, "Orden obtenida exitosamente"));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(new ApiResponseDto<IEnumerable<OrderResponseDto>>(orders, "Órdenes del usuario obtenidas exitosamente"));
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(new ApiResponseDto<IEnumerable<OrderResponseDto>>(orders, $"Órdenes con estado {status} obtenidas exitosamente"));
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetByDateRange(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(new ApiResponseDto<IEnumerable<OrderResponseDto>>(orders, "Órdenes por rango de fechas obtenidas exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<OrderResponseDto>>>> GetPaged([FromQuery] OrderFilterDto filter)
        {
            var result = await _orderService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<OrderResponseDto>>(result, "Órdenes paginadas obtenidas exitosamente"));
        }

        [HttpGet("recent/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetRecent(int count)
        {
            var orders = await _orderService.GetRecentOrdersAsync(count);
            return Ok(new ApiResponseDto<IEnumerable<OrderResponseDto>>(orders, "Órdenes recientes obtenidas exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> Create([FromBody] OrderCreateDto createDto)
        {
            var order = await _orderService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id },
                new ApiResponseDto<OrderResponseDto>(order, "Orden creada exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> Update(int id, [FromBody] OrderUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<OrderResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var order = await _orderService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<OrderResponseDto>(order, "Orden actualizada exitosamente"));
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> UpdateStatus(int id, [FromBody] OrderStatus status)
        {
            var order = await _orderService.UpdateStatusAsync(id, status);
            return Ok(new ApiResponseDto<OrderResponseDto>(order, "Estado de la orden actualizado exitosamente"));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> CancelOrder(int id, [FromBody] string reason)
        {
            var order = await _orderService.CancelOrderAsync(id, reason);
            return Ok(new ApiResponseDto<OrderResponseDto>(order, "Orden cancelada exitosamente"));
        }

        [HttpPatch("{id}/shipping")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UpdateShippingInfo(
            int id, [FromQuery] string trackingNumber, [FromQuery] string shippingMethod)
        {
            var result = await _orderService.UpdateShippingInfoAsync(id, trackingNumber, shippingMethod);
            return Ok(new ApiResponseDto<bool>(result, "Información de envío actualizada exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Orden eliminada exitosamente"));
        }

        [HttpGet("count-by-status/{status}")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetCountByStatus(OrderStatus status)
        {
            var count = await _orderService.GetOrderCountByStatusAsync(status);
            return Ok(new ApiResponseDto<int>(count, $"Conteo de órdenes con estado {status} obtenido exitosamente"));
        }

        [HttpGet("total-sales")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalSales(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var total = await _orderService.GetTotalSalesAsync(startDate, endDate);
            return Ok(new ApiResponseDto<decimal>(total, "Total de ventas obtenido exitosamente"));
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponseDto<OrderStatisticsDto>>> GetStatistics(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _orderService.GetOrderStatisticsAsync(startDate, endDate);
            return Ok(new ApiResponseDto<OrderStatisticsDto>(stats, "Estadísticas de órdenes obtenidas exitosamente"));
        }
    }
}