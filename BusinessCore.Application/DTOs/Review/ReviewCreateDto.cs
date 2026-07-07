namespace BusinessCore.Application.DTOs.Review
{
    public class ReviewCreateDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool IsVerifiedPurchase { get; set; } = false;
    }
}