using AutoMapper;
using BusinessCore.Application.DTOs.Audit;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class AuditMappingProfile : Profile
    {
        public AuditMappingProfile()
        {
            CreateMap<AuditLogDto, AuditLog>();
            CreateMap<AuditLog, AuditLogDto>();
        }
    }
}