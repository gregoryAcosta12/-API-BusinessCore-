using System;

namespace BusinessCore.Application.DTOs.Address
{
    public class AddressResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressType { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}