using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Products;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Producto con ID {id} no encontrado");

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<PagedResultDto<ProductResponseDto>> GetPagedAsync(ProductFilterDto filter)
        {
            var products = await _repository.GetPagedWithFiltersAsync(
                filter.PageNumber,
                filter.PageSize,
                filter.Name,
                filter.CategoryId,
                filter.BrandId,
                filter.SortBy,
                filter.SortAscending
            );

            var totalCount = await _repository.GetCountWithFiltersAsync(
                filter.Name,
                filter.CategoryId,
                filter.BrandId,
                filter.MinPrice,
                filter.MaxPrice
            );

            var items = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            return new PagedResultDto<ProductResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<ProductResponseDto> CreateAsync(ProductCreateDto createDto)
        {
            if (await _repository.ExistsBySkuAsync(createDto.Sku))
                throw new ConflictException($"El SKU {createDto.Sku} ya existe");

            var product = _mapper.Map<Product>(createDto);
            product.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(product);
            return _mapper.Map<ProductResponseDto>(created);
        }

        public async Task<ProductResponseDto> UpdateAsync(ProductUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Producto con ID {updateDto.Id} no encontrado");

            if (existing.Sku != updateDto.Sku && await _repository.ExistsBySkuAsync(updateDto.Sku))
                throw new ConflictException($"El SKU {updateDto.Sku} ya existe");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<ProductResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Producto con ID {id} no encontrado");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByBrandAsync(int brandId)
        {
            var products = await _repository.GetByBrandAsync(brandId);
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByNameAsync(string name)
        {
            var products = await _repository.GetByNameAsync(name);
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetLowStockAsync(int threshold)
        {
            var products = await _repository.GetLowStockAsync(threshold);
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetFeaturedAsync()
        {
            var products = await _repository.GetFeaturedAsync();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            if (quantity < 0)
                throw new BadRequestException("La cantidad no puede ser negativa");

            return await _repository.UpdateStockAsync(productId, quantity);
        }

        public async Task<bool> UpdatePriceAsync(int productId, decimal newPrice)
        {
            if (newPrice < 0)
                throw new BadRequestException("El precio no puede ser negativo");

            return await _repository.UpdatePriceAsync(productId, newPrice);
        }

        public async Task<bool> BulkUpdatePricesAsync(List<ProductUpdateDto> products)
        {
            if (products == null || !products.Any())
                throw new BadRequestException("La lista de productos está vacía");

            var productEntities = _mapper.Map<List<Product>>(products);
            return await _repository.BulkUpdatePricesAsync(productEntities);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _repository.GetCountAsync();
        }

        public async Task<decimal> GetAveragePriceAsync()
        {
            var products = await _repository.GetAllAsync();
            if (!products.Any())
                return 0;

            return products.Average(p => p.Price);
        }
    }
}