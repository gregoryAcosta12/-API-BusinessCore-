using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Products;

namespace BusinessCore.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<PagedResultDto<ProductResponseDto>> GetPagedAsync(ProductFilterDto filter);
        Task<ProductResponseDto> CreateAsync(ProductCreateDto createDto);
        Task<ProductResponseDto> UpdateAsync(ProductUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDto>> GetByBrandAsync(int brandId);
        Task<IEnumerable<ProductResponseDto>> GetByNameAsync(string name);
        Task<IEnumerable<ProductResponseDto>> GetLowStockAsync(int threshold);
        Task<IEnumerable<ProductResponseDto>> GetFeaturedAsync();
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<bool> UpdatePriceAsync(int productId, decimal newPrice);
        Task<bool> BulkUpdatePricesAsync(List<ProductUpdateDto> products);
        Task<int> GetTotalCountAsync();
        Task<decimal> GetAveragePriceAsync();
    }
}