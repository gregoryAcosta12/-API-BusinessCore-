using AutoMapper;
using BusinessCore.Application.DTOs.Customer;
using BusinessCore.Application.DTOs.Customers;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Count : 0))
                .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Where(o => o.Status == Domain.Enums.OrderStatus.Delivered).Sum(o => o.TotalAmount) : 0));

            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}