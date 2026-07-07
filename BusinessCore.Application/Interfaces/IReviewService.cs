using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Review;
using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.Interfaces
{
   
    public interface IReviewService
    {
       
        Task<ReviewResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<ReviewResponseDto>> GetProductReviewsAsync(int productId);
        Task<IEnumerable<ReviewResponseDto>> GetUserReviewsAsync(int userId);
        Task<PagedResultDto<ReviewResponseDto>> GetPagedAsync(ReviewFilterDto filter);
        Task<ReviewResponseDto> CreateAsync(ReviewCreateDto createDto);
        Task<ReviewResponseDto> UpdateAsync(ReviewUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

       
        Task<ReviewResponseDto> ApproveReviewAsync(int reviewId);
        Task<ReviewResponseDto> RejectReviewAsync(int reviewId, string reason);
        Task<IEnumerable<ReviewResponseDto>> GetPendingReviewsAsync();

       
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetReviewCountAsync(int productId);
        Task<RatingDistributionDto> GetRatingDistributionAsync(int productId);

        Task<IEnumerable<ReviewResponseDto>> GetTopReviewsAsync(int productId, int count);
        Task<bool> ReportReviewAsync(int reviewId, string reason);
        Task<bool> MarkAsVerifiedPurchaseAsync(int reviewId);
    }

    public class RatingDistributionDto
    {
        public int Rating5Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating1Count { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}