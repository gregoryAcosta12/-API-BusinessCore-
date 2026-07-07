using AutoMapper;
using BusinessCore.Application.DTOs.User;
using BusinessCore.Application.DTOs.Users;
using BusinessCore.Domain.Entities;

namespace BusinessCore.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Select(ur => ur.Role.Name).ToList() : new List<string>()));

            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}