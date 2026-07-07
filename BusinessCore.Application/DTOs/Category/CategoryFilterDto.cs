namespace BusinessCore.Application.DTOs.Category
{
    public class CategoryFilterDto
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public bool IncludeChildCategories { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}