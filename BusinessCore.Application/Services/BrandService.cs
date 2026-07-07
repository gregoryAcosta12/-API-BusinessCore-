using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Brand;
using BusinessCore.Application.DTOs.Brands;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BrandResponseDto> GetByIdAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException($"Marca con ID {id} no encontrada");

            return _mapper.Map<BrandResponseDto>(brand);
        }

        public async Task<IEnumerable<BrandResponseDto>> GetAllAsync()
        {
            var brands = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandResponseDto>>(brands);
        }

        public async Task<IEnumerable<BrandResponseDto>> GetActiveAsync()
        {
            var brands = await _repository.GetActiveAsync();
            return _mapper.Map<IEnumerable<BrandResponseDto>>(brands);
        }

        public async Task<PagedResultDto<BrandResponseDto>> GetPagedAsync(BrandFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var brands = await query;

            if (!string.IsNullOrEmpty(filter.Name))
                brands = brands.Where(b => b.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));

            if (filter.IsActive.HasValue)
                brands = brands.Where(b => b.IsActive == filter.IsActive);

            var totalCount = brands.Count();
            var items = brands
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<BrandResponseDto>
            {
                Items = _mapper.Map<IEnumerable<BrandResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<BrandResponseDto> CreateAsync(BrandCreateDto createDto)
        {
            if (await _repository.ExistsByNameAsync(createDto.Name))
                throw new ConflictException($"La marca {createDto.Name} ya existe");

            var brand = _mapper.Map<Brand>(createDto);
            brand.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(brand);
            return _mapper.Map<BrandResponseDto>(created);
        }

        public async Task<BrandResponseDto> UpdateAsync(BrandUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Marca con ID {updateDto.Id} no encontrada");

            if (existing.Name != updateDto.Name && await _repository.ExistsByNameAsync(updateDto.Name))
                throw new ConflictException($"La marca {updateDto.Name} ya existe");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<BrandResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Marca con ID {id} no encontrada");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repository.ExistsByNameAsync(name);
        }

        public async Task<IEnumerable<BrandResponseDto>> GetTopBrandsAsync(int count)
        {
            var brands = await _repository.GetTopBrandsAsync(count);
            return _mapper.Map<IEnumerable<BrandResponseDto>>(brands);
        }

        public async Task<int> GetProductCountAsync(int brandId)
        {
            return await _repository.GetProductCountAsync(brandId);
        }

        public async Task<bool> BulkDeleteAsync(List<int> brandIds)
        {
            foreach (var id in brandIds)
            {
                var brand = await _repository.GetByIdAsync(id);
                if (brand != null)
                {
                    brand.IsActive = false;
                    brand.UpdatedAt = DateTime.UtcNow;
                    await _repository.UpdateAsync(brand);
                }
            }
            return true;
        }
    }
}