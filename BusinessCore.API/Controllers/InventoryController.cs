using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Inventory;
using BusinessCore.Application.Interfaces;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("movement/{id}")]
        public async Task<ActionResult<ApiResponseDto<InventoryMovementResponseDto>>> GetMovementById(int id)
        {
            var movement = await _inventoryService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<InventoryMovementResponseDto>(movement, "Movimiento de inventario obtenido exitosamente"));
        }

        [HttpGet("product/{productId}/movements")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<InventoryMovementResponseDto>>>> GetProductMovements(int productId)
        {
            var movements = await _inventoryService.GetMovementsByProductIdAsync(productId);
            return Ok(new ApiResponseDto<IEnumerable<InventoryMovementResponseDto>>(movements, "Movimientos del producto obtenidos exitosamente"));
        }

        [HttpGet("movements/date-range")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<InventoryMovementResponseDto>>>> GetMovementsByDateRange(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var movements = await _inventoryService.GetMovementsByDateRangeAsync(startDate, endDate);
            return Ok(new ApiResponseDto<IEnumerable<InventoryMovementResponseDto>>(movements, "Movimientos por rango de fechas obtenidos exitosamente"));
        }

        [HttpGet("movements/paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<InventoryMovementResponseDto>>>> GetMovementsPaged([FromQuery] InventoryFilterDto filter)
        {
            var result = await _inventoryService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<InventoryMovementResponseDto>>(result, "Movimientos paginados obtenidos exitosamente"));
        }

        [HttpPost("movement")]
        public async Task<ActionResult<ApiResponseDto<InventoryMovementResponseDto>>> CreateMovement([FromBody] InventoryMovementCreateDto createDto)
        {
            var movement = await _inventoryService.CreateMovementAsync(createDto);
            return CreatedAtAction(nameof(GetMovementById), new { id = movement.Id },
                new ApiResponseDto<InventoryMovementResponseDto>(movement, "Movimiento de inventario creado exitosamente"));
        }

        [HttpPost("adjust-stock/{productId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AdjustStock(int productId, [FromBody] AdjustStockDto adjustDto)
        {
            var result = await _inventoryService.AdjustStockAsync(productId, adjustDto.Quantity, adjustDto.Reason);
            return Ok(new ApiResponseDto<bool>(result, "Stock ajustado exitosamente"));
        }

        [HttpPost("transfer-stock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> TransferStock([FromBody] TransferStockDto transferDto)
        {
            var result = await _inventoryService.TransferStockAsync(
                transferDto.ProductId,
                transferDto.SourceWarehouseId,
                transferDto.TargetWarehouseId,
                transferDto.Quantity);
            return Ok(new ApiResponseDto<bool>(result, "Transferencia de stock realizada exitosamente"));
        }

        [HttpPost("receive-stock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ReceiveStock([FromBody] ReceiveStockDto receiveDto)
        {
            var result = await _inventoryService.ReceiveStockAsync(
                receiveDto.ProductId,
                receiveDto.Quantity,
                receiveDto.UnitCost,
                receiveDto.Reference);
            return Ok(new ApiResponseDto<bool>(result, "Recepción de stock realizada exitosamente"));
        }

        [HttpPost("release-stock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ReleaseStock([FromBody] ReleaseStockDto releaseDto)
        {
            var result = await _inventoryService.ReleaseStockAsync(
                releaseDto.ProductId,
                releaseDto.Quantity,
                releaseDto.Reference);
            return Ok(new ApiResponseDto<bool>(result, "Liberación de stock realizada exitosamente"));
        }

        [HttpGet("{productId}/stock")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetCurrentStock(int productId)
        {
            var stock = await _inventoryService.GetCurrentStockAsync(productId);
            return Ok(new ApiResponseDto<int>(stock, "Stock actual obtenido exitosamente"));
        }

        [HttpGet("{productId}/stock/warehouse/{warehouseId}")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetCurrentStockByWarehouse(int productId, int warehouseId)
        {
            var stock = await _inventoryService.GetCurrentStockAsync(productId, warehouseId);
            return Ok(new ApiResponseDto<int>(stock, "Stock actual por almacén obtenido exitosamente"));
        }

        [HttpGet("low-stock/{threshold}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductStockDto>>>> GetLowStock(int threshold)
        {
            var products = await _inventoryService.GetLowStockProductsAsync(threshold);
            return Ok(new ApiResponseDto<IEnumerable<ProductStockDto>>(products, "Productos con bajo stock obtenidos exitosamente"));
        }

        [HttpGet("products-with-stock")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductStockDto>>>> GetProductsWithStock()
        {
            var products = await _inventoryService.GetProductsWithStockAsync();
            return Ok(new ApiResponseDto<IEnumerable<ProductStockDto>>(products, "Productos con stock obtenidos exitosamente"));
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponseDto<InventoryStatisticsDto>>> GetStatistics()
        {
            var stats = await _inventoryService.GetInventoryStatisticsAsync();
            return Ok(new ApiResponseDto<InventoryStatisticsDto>(stats, "Estadísticas de inventario obtenidas exitosamente"));
        }

        [HttpGet("total-value")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalValue()
        {
            var value = await _inventoryService.GetTotalInventoryValueAsync();
            return Ok(new ApiResponseDto<decimal>(value, "Valor total del inventario obtenido exitosamente"));
        }

        [HttpGet("warehouse-summary")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<WarehouseStockDto>>>> GetWarehouseSummary()
        {
            var summary = await _inventoryService.GetWarehouseStockSummaryAsync();
            return Ok(new ApiResponseDto<IEnumerable<WarehouseStockDto>>(summary, "Resumen por almacén obtenido exitosamente"));
        }

        [HttpDelete("movement/{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteMovement(int id)
        {
            var result = await _inventoryService.DeleteMovementAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Movimiento de inventario eliminado exitosamente"));
        }
    }

    public class AdjustStockDto
    {
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }

    public class TransferStockDto
    {
        public int ProductId { get; set; }
        public int SourceWarehouseId { get; set; }
        public int TargetWarehouseId { get; set; }
        public int Quantity { get; set; }
    }

    public class ReceiveStockDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string Reference { get; set; }
    }

    public class ReleaseStockDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reference { get; set; }
    }
}