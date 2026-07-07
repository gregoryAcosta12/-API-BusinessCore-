using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Categories;
using BusinessCore.Application.DTOs.Category;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Categoría con ID {id} no encontrada");

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> GetBySlugAsync(string slug)
        {
            var category = await _repository.GetBySlugAsync(slug);
            if (category == null)
                throw new NotFoundException($"Categoría con slug {slug} no encontrada");

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetActiveAsync()
        {
            var categories = await _repository.GetActiveAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<PagedResultDto<CategoryResponseDto>> GetPagedAsync(CategoryFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var categories = await query;

            if (!string.IsNullOrEmpty(filter.Name))
                categories = categories.Where(c => c.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));

            if (filter.ParentCategoryId.HasValue)
                categories = categories.Where(c => c.ParentCategoryId == filter.ParentCategoryId);

            if (filter.IsActive.HasValue)
                categories = categories.Where(c => c.IsActive == filter.IsActive);

            var totalCount = categories.Count();
            var items = categories
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<CategoryResponseDto>
            {
                Items = _mapper.Map<IEnumerable<CategoryResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto createDto)
        {
            if (await _repository.ExistsByNameAsync(createDto.Name))
                throw new ConflictException($"La categoría {createDto.Name} ya existe");

            if (!string.IsNullOrEmpty(createDto.Slug) && await _repository.GetBySlugAsync(createDto.Slug) != null)
                throw new ConflictException($"El slug {createDto.Slug} ya existe");

            var category = _mapper.Map<Category>(createDto);
            category.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(category);
            return _mapper.Map<CategoryResponseDto>(created);
        }

        public async Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Categoría con ID {updateDto.Id} no encontrada");

            if (existing.Name != updateDto.Name && await _repository.ExistsByNameAsync(updateDto.Name))
                throw new ConflictException($"La categoría {updateDto.Name} ya existe");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<CategoryResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Categoría con ID {id} no encontrada");

            var childCount = await _repository.GetSubCategoriesAsync(id);
            if (childCount.Any())
                throw new BusinessException("No se puede eliminar una categoría que tiene subcategorías");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetMainCategoriesAsync()
        {
            var categories = await _repository.GetMainCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetSubCategoriesAsync(int parentId)
        {
            var categories = await _repository.GetSubCategoriesAsync(parentId);
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoryHierarchyAsync()
        {
            var categories = await _repository.GetCategoryHierarchyAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            return await _repository.GetProductCountAsync(categoryId);
        }

        public async Task<bool> ReorderCategoriesAsync(Dictionary<int, int> orderMapping)
        {
            foreach (var item in orderMapping)
            {
                var category = await _repository.GetByIdAsync(item.Key);
                if (category != null)
                {
                    category.DisplayOrder = item.Value;
                    category.UpdatedAt = DateTime.UtcNow;
                    await _repository.UpdateAsync(category);
                }
            }
            return true;
        }

        public async Task<bool> BulkUpdateStatusAsync(List<int> categoryIds, bool isActive)
        {
            foreach (var id in categoryIds)
            {
                var category = await _repository.GetByIdAsync(id);
                if (category != null)
                {
                    category.IsActive = isActive;
                    category.UpdatedAt = DateTime.UtcNow;
                    await _repository.UpdateAsync(category);
                }
            }
            return true;
        }
    }
}