using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Products;
using BusinessCore.Application.Interfaces;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos obtenidos exitosamente"));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetActive()
        {
            var products = await _productService.GetActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos activos obtenidos exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<ProductResponseDto>(product, "Producto obtenido exitosamente"));
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetByCategory(int categoryId)
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos por categoría obtenidos exitosamente"));
        }

        [HttpGet("by-brand/{brandId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetByBrand(int brandId)
        {
            var products = await _productService.GetByBrandAsync(brandId);
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos por marca obtenidos exitosamente"));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> Search([FromQuery] string name)
        {
            var products = await _productService.GetByNameAsync(name);
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Búsqueda de productos exitosa"));
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetLowStock([FromQuery] int threshold = 10)
        {
            var products = await _productService.GetLowStockAsync(threshold);
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos con bajo stock obtenidos exitosamente"));
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetFeatured()
        {
            var products = await _productService.GetFeaturedAsync();
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos destacados obtenidos exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<ProductResponseDto>>>> GetPaged([FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<ProductResponseDto>>(result, "Productos paginados obtenidos exitosamente"));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetWithFilters([FromQuery] ProductFilterDto filter)
        {
            var products = await _productService.GetProductsWithFiltersAsync(filter);
            return Ok(new ApiResponseDto<IEnumerable<ProductResponseDto>>(products, "Productos filtrados obtenidos exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> Create([FromBody] ProductCreateDto createDto)
        {
            var product = await _productService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id },
                new ApiResponseDto<ProductResponseDto>(product, "Producto creado exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> Update(int id, [FromBody] ProductUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<ProductResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var product = await _productService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<ProductResponseDto>(product, "Producto actualizado exitosamente"));
        }

        [HttpPatch("{id}/stock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UpdateStock(int id, [FromBody] int quantity)
        {
            var result = await _productService.UpdateStockAsync(id, quantity);
            return Ok(new ApiResponseDto<bool>(result, "Stock actualizado exitosamente"));
        }

        [HttpPatch("{id}/price")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UpdatePrice(int id, [FromBody] decimal price)
        {
            var result = await _productService.UpdatePriceAsync(id, price);
            return Ok(new ApiResponseDto<bool>(result, "Precio actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Producto eliminado exitosamente"));
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Exists(int id)
        {
            var result = await _productService.ExistsAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Verificación de existencia exitosa"));
        }

        [HttpGet("count")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetTotalCount()
        {
            var count = await _productService.GetTotalCountAsync();
            return Ok(new ApiResponseDto<int>(count, "Total de productos obtenido exitosamente"));
        }

        [HttpGet("average-price")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetAveragePrice()
        {
            var average = await _productService.GetAveragePriceAsync();
            return Ok(new ApiResponseDto<decimal>(average, "Precio promedio obtenido exitosamente"));
        }
    }
}