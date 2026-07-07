using AutoMapper;
using BusinessCore.Application.DTOs.Payment;
using BusinessCore.Application.DTOs.Payments;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderNumber : null));

            CreateMap<PaymentCreateDto, Payment>();
            CreateMap<PaymentProcessDto, Payment>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId));

            CreateMap<PaymentUpdateDto, Payment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}