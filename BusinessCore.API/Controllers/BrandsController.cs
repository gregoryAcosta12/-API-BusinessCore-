using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Brand;
using BusinessCore.Application.DTOs.Brands;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<BrandResponseDto>>>> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<BrandResponseDto>>(brands, "Marcas obtenidas exitosamente"));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<BrandResponseDto>>>> GetActive()
        {
            var brands = await _brandService.GetActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<BrandResponseDto>>(brands, "Marcas activas obtenidas exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<BrandResponseDto>>> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<BrandResponseDto>(brand, "Marca obtenida exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<BrandResponseDto>>>> GetPaged([FromQuery] BrandFilterDto filter)
        {
            var result = await _brandService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<BrandResponseDto>>(result, "Marcas paginadas obtenidas exitosamente"));
        }

        [HttpGet("top/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<BrandResponseDto>>>> GetTopBrands(int count)
        {
            var brands = await _brandService.GetTopBrandsAsync(count);
            return Ok(new ApiResponseDto<IEnumerable<BrandResponseDto>>(brands, "Marcas top obtenidas exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<BrandResponseDto>>> Create([FromBody] BrandCreateDto createDto)
        {
            var brand = await _brandService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = brand.Id },
                new ApiResponseDto<BrandResponseDto>(brand, "Marca creada exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<BrandResponseDto>>> Update(int id, [FromBody] BrandUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<BrandResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var brand = await _brandService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<BrandResponseDto>(brand, "Marca actualizada exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Marca eliminada exitosamente"));
        }

        [HttpDelete("bulk")]
        public async Task<ActionResult<ApiResponseDto<bool>>> BulkDelete([FromBody] List<int> ids)
        {
            var result = await _brandService.BulkDeleteAsync(ids);
            return Ok(new ApiResponseDto<bool>(result, "Marcas eliminadas exitosamente"));
        }

        [HttpGet("{id}/product-count")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetProductCount(int id)
        {
            var count = await _brandService.GetProductCountAsync(id);
            return Ok(new ApiResponseDto<int>(count, "Conteo de productos obtenido exitosamente"));
        }

        [HttpGet("exists/{name}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ExistsByName(string name)
        {
            var result = await _brandService.ExistsByNameAsync(name);
            return Ok(new ApiResponseDto<bool>(result, "Verificación de existencia exitosa"));
        }
    }
}