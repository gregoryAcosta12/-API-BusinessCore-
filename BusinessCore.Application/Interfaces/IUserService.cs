using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.User;

namespace BusinessCore.Application.Interfaces
{

    public interface IUserService
    {
       
        Task<UserResponseDto> GetByIdAsync(int id);
        Task<UserResponseDto> GetByEmailAsync(string email);
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<IEnumerable<UserResponseDto>> GetActiveAsync();
        Task<PagedResultDto<UserResponseDto>> GetPagedAsync(UserFilterDto filter);
        Task<UserResponseDto> CreateAsync(UserCreateDto createDto);
        Task<UserResponseDto> UpdateAsync(UserUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);

        
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(string email);

      
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
        Task<bool> HasRoleAsync(int userId, string roleName);

       
        Task<bool> ConfirmEmailAsync(int userId, string token);
        Task<bool> ResendConfirmationEmailAsync(string email);
        Task<bool> LockUserAsync(int userId);
        Task<bool> UnlockUserAsync(int userId);
    }
}