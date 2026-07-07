using AutoMapper;
using BusinessCore.Application.DTOs.Brand;
using BusinessCore.Application.DTOs.Brands;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            CreateMap<Brand, BrandResponseDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

            CreateMap<BrandCreateDto, Brand>();
            CreateMap<BrandUpdateDto, Brand>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}