using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Category;
using BusinessCore.Application.DTOs.Common;

namespace BusinessCore.Application.Interfaces
{
  
    public interface ICategoryService
    {
        // CRUD Básico
        Task<CategoryResponseDto> GetByIdAsync(int id);
        Task<CategoryResponseDto> GetBySlugAsync(string slug);
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
        Task<IEnumerable<CategoryResponseDto>> GetActiveAsync();
        Task<PagedResultDto<CategoryResponseDto>> GetPagedAsync(CategoryFilterDto filter);
        Task<CategoryResponseDto> CreateAsync(CategoryCreateDto createDto);
        Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Consultas específicas
        Task<IEnumerable<CategoryResponseDto>> GetMainCategoriesAsync();
        Task<IEnumerable<CategoryResponseDto>> GetSubCategoriesAsync(int parentId);
        Task<IEnumerable<CategoryResponseDto>> GetCategoryHierarchyAsync();
        Task<int> GetProductCountAsync(int categoryId);

        // Operaciones masivas
        Task<bool> ReorderCategoriesAsync(Dictionary<int, int> orderMapping);
        Task<bool> BulkUpdateStatusAsync(List<int> categoryIds, bool isActive);
    }
}