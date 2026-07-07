namespace BusinessCore.Application.DTOs.Customer
{
    public class CustomerUpdateDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string BusinessType { get; set; }
        public decimal CreditLimit { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsActive { get; set; }
    }
}