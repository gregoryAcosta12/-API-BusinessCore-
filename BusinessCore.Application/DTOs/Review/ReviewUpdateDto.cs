using BusinessCore.Domain.Enums;

namespace BusinessCore.Application.DTOs.Review
{
    public class ReviewUpdateDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
        public bool IsActive { get; set; }
    }
}