namespace BusinessCore.Application.DTOs.Customer
{
    public class CustomerCreateDto
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string BusinessType { get; set; }
        public decimal CreditLimit { get; set; } = 0;
        public string PaymentTerms { get; set; }
    }
}