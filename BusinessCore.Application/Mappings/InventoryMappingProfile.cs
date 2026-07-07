using AutoMapper;
using BusinessCore.Application.DTOs.Inventory;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            CreateMap<InventoryMovement, InventoryMovementResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
                .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.ProductVariant != null ? src.ProductVariant.Name : null))
                .ForMember(dest => dest.SourceWarehouseName, opt => opt.MapFrom(src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : null))
                .ForMember(dest => dest.TargetWarehouseName, opt => opt.MapFrom(src => src.TargetWarehouse != null ? src.TargetWarehouse.Name : null));

            CreateMap<InventoryMovementCreateDto, InventoryMovement>();
        }
    }
}