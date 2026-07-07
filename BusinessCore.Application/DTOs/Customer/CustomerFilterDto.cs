namespace BusinessCore.Application.DTOs.Customer
{
    public class CustomerFilterDto
    {
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string BusinessType { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasBalance { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}