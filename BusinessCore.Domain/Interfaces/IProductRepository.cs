using BusinessCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IProductRepository
    {
        
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetActiveAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetBySkuAsync(string sku);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsBySkuAsync(string sku);

        // Consultas específicas
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByBrandAsync(int brandId);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold);
        Task<IEnumerable<Product>> GetFeaturedAsync();
        Task<IEnumerable<Product>> GetProductsWithFiltersAsync(
            string name = null,
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null);

        Task<int> GetCountAsync();
        Task<int> GetCountWithFiltersAsync(
            string name = null,
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null);

        // Inventario
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<bool> UpdatePriceAsync(int productId, decimal newPrice);
        Task<bool> BulkUpdatePricesAsync(List<Product> products);

        // Paginación
        Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Product>> GetPagedWithFiltersAsync(
            int pageNumber,
            int pageSize,
            string name = null,
            int? categoryId = null,
            int? brandId = null,
            string sortBy = null,
            bool sortAscending = true);
    }
}