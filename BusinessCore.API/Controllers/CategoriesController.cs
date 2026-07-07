using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Categories;
using BusinessCore.Application.DTOs.Category;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CategoryResponseDto>>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<CategoryResponseDto>>(categories, "Categorías obtenidas exitosamente"));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CategoryResponseDto>>>> GetActive()
        {
            var categories = await _categoryService.GetActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<CategoryResponseDto>>(categories, "Categorías activas obtenidas exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<CategoryResponseDto>>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<CategoryResponseDto>(category, "Categoría obtenida exitosamente"));
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ApiResponseDto<CategoryResponseDto>>> GetBySlug(string slug)
        {
            var category = await _categoryService.GetBySlugAsync(slug);
            return Ok(new ApiResponseDto<CategoryResponseDto>(category, "Categoría obtenida exitosamente"));
        }

        [HttpGet("main")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CategoryResponseDto>>>> GetMainCategories()
        {
            var categories = await _categoryService.GetMainCategoriesAsync();
            return Ok(new ApiResponseDto<IEnumerable<CategoryResponseDto>>(categories, "Categorías principales obtenidas exitosamente"));
        }

        [HttpGet("{parentId}/subcategories")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CategoryResponseDto>>>> GetSubCategories(int parentId)
        {
            var categories = await _categoryService.GetSubCategoriesAsync(parentId);
            return Ok(new ApiResponseDto<IEnumerable<CategoryResponseDto>>(categories, "Subcategorías obtenidas exitosamente"));
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CategoryResponseDto>>>> GetHierarchy()
        {
            var hierarchy = await _categoryService.GetCategoryHierarchyAsync();
            return Ok(new ApiResponseDto<IEnumerable<CategoryResponseDto>>(hierarchy, "Jerarquía de categorías obtenida exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<CategoryResponseDto>>>> GetPaged([FromQuery] CategoryFilterDto filter)
        {
            var result = await _categoryService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<CategoryResponseDto>>(result, "Categorías paginadas obtenidas exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<CategoryResponseDto>>> Create([FromBody] CategoryCreateDto createDto)
        {
            var category = await _categoryService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id },
                new ApiResponseDto<CategoryResponseDto>(category, "Categoría creada exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<CategoryResponseDto>>> Update(int id, [FromBody] CategoryUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<CategoryResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var category = await _categoryService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<CategoryResponseDto>(category, "Categoría actualizada exitosamente"));
        }

        [HttpPatch("reorder")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Reorder([FromBody] Dictionary<int, int> orderMapping)
        {
            var result = await _categoryService.ReorderCategoriesAsync(orderMapping);
            return Ok(new ApiResponseDto<bool>(result, "Orden de categorías actualizado exitosamente"));
        }

        [HttpPatch("bulk-status")]
        public async Task<ActionResult<ApiResponseDto<bool>>> BulkUpdateStatus([FromBody] BulkStatusDto bulkStatusDto)
        {
            var result = await _categoryService.BulkUpdateStatusAsync(bulkStatusDto.Ids, bulkStatusDto.IsActive);
            return Ok(new ApiResponseDto<bool>(result, "Estado de categorías actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Categoría eliminada exitosamente"));
        }

        [HttpGet("{id}/product-count")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetProductCount(int id)
        {
            var count = await _categoryService.GetProductCountAsync(id);
            return Ok(new ApiResponseDto<int>(count, "Conteo de productos obtenido exitosamente"));
        }
    }

    public class BulkStatusDto
    {
        public List<int> Ids { get; set; }
        public bool IsActive { get; set; }
    }
}