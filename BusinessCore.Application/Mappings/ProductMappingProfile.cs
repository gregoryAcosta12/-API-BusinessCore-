using AutoMapper;
using BusinessCore.Application.DTOs.Product;
using BusinessCore.Application.DTOs.Products;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

            CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductImageCreateDto, ProductImage>();

            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<ProductVariantCreateDto, ProductVariant>();
        }
    }
}