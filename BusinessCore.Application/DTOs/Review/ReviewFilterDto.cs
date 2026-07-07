using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Review
{
    public class ReviewFilterDto
    {
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public ReviewStatus? Status { get; set; }
        public bool? IsVerifiedPurchase { get; set; }
        public bool? IsActive { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}