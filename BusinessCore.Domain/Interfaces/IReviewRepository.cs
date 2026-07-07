using BusinessCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task<IEnumerable<Review>> GetUserReviewsAsync(int userId);
        Task<IEnumerable<Review>> GetPendingReviewsAsync();
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetReviewCountAsync(int productId);
        Task<IEnumerable<Review>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Review>> GetProductReviewsPagedAsync(int productId, int pageNumber, int pageSize);
    }
}