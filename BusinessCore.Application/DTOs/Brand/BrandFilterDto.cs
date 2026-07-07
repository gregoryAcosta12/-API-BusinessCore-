namespace BusinessCore.Application.DTOs.Brand
{
    public class BrandFilterDto
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}