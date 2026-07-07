using AutoMapper;
using BusinessCore.Application.DTOs.Address;
using BusinessCore.Application.DTOs.Addresses;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<Address, AddressResponseDto>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}