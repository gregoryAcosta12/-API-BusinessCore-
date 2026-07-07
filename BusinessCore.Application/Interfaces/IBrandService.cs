using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Brand;
using BusinessCore.Application.DTOs.Common;

namespace BusinessCore.Application.Interfaces
{
 
    public interface IBrandService
    {
       
        Task<BrandResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<BrandResponseDto>> GetAllAsync();
        Task<IEnumerable<BrandResponseDto>> GetActiveAsync();
        Task<PagedResultDto<BrandResponseDto>> GetPagedAsync(BrandFilterDto filter);
        Task<BrandResponseDto> CreateAsync(BrandCreateDto createDto);
        Task<BrandResponseDto> UpdateAsync(BrandUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);

    
        Task<IEnumerable<BrandResponseDto>> GetTopBrandsAsync(int count);
        Task<int> GetProductCountAsync(int brandId);

   
        Task<bool> BulkDeleteAsync(List<int> brandIds);
    }
}