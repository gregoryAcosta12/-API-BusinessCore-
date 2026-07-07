using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Role;
using BusinessCore.Application.DTOs.User;

namespace BusinessCore.Application.Interfaces
{
>
    public interface IRoleService
    {
        
        Task<RoleResponseDto> GetByIdAsync(int id);
        Task<RoleResponseDto> GetByNameAsync(string name);
        Task<IEnumerable<RoleResponseDto>> GetAllAsync();
        Task<IEnumerable<RoleResponseDto>> GetActiveAsync();
        Task<PagedResultDto<RoleResponseDto>> GetPagedAsync(RoleFilterDto filter);
        Task<RoleResponseDto> CreateAsync(RoleCreateDto createDto);
        Task<RoleResponseDto> UpdateAsync(RoleUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);

        Task<bool> AssignPermissionAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionAsync(int roleId, int permissionId);
        Task<IEnumerable<string>> GetRolePermissionsAsync(int roleId);
        Task<bool> HasPermissionAsync(int roleId, string permission);

        
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<IEnumerable<UserResponseDto>> GetUsersInRoleAsync(int roleId);
    }
}