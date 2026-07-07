using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Roles;
using BusinessCore.Application.DTOs.User;
using BusinessCore.Application.DTOs.Users;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RoleResponseDto> GetByIdAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                throw new NotFoundException($"Rol con ID {id} no encontrado");

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> GetByNameAsync(string name)
        {
            var role = await _repository.GetByNameAsync(name);
            if (role == null)
                throw new NotFoundException($"Rol con nombre {name} no encontrado");

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
        }

        public async Task<IEnumerable<RoleResponseDto>> GetActiveAsync()
        {
            var roles = await _repository.GetAllAsync();
            var activeRoles = roles.Where(r => r.IsActive);
            return _mapper.Map<IEnumerable<RoleResponseDto>>(activeRoles);
        }

        public async Task<PagedResultDto<RoleResponseDto>> GetPagedAsync(RoleFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var roles = await query;

            if (!string.IsNullOrEmpty(filter.Name))
                roles = roles.Where(r => r.Name.Contains(filter.Name));

            if (filter.IsActive.HasValue)
                roles = roles.Where(r => r.IsActive == filter.IsActive);

            var totalCount = roles.Count();
            var items = roles
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<RoleResponseDto>
            {
                Items = _mapper.Map<IEnumerable<RoleResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<RoleResponseDto> CreateAsync(RoleCreateDto createDto)
        {
            if (await _repository.ExistsByNameAsync(createDto.Name))
                throw new ConflictException($"El rol {createDto.Name} ya existe");

            var role = _mapper.Map<Role>(createDto);
            role.IsActive = true;

            var created = await _repository.CreateAsync(role);
            return _mapper.Map<RoleResponseDto>(created);
        }

        public async Task<RoleResponseDto> UpdateAsync(RoleUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Rol con ID {updateDto.Id} no encontrado");

            if (existing.Name != updateDto.Name && await _repository.ExistsByNameAsync(updateDto.Name))
                throw new ConflictException($"El rol {updateDto.Name} ya existe");

            _mapper.Map(updateDto, existing);

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<RoleResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Rol con ID {id} no encontrado");

            if (existing.Name == "SuperAdmin")
                throw new BusinessException("No se puede eliminar el rol SuperAdmin");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repository.ExistsByNameAsync(name);
        }

        public async Task<bool> AssignPermissionAsync(int roleId, int permissionId)
        {
           
            return true;
        }

        public async Task<bool> RemovePermissionAsync(int roleId, int permissionId)
        {
            
            return true;
        }

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(int roleId)
        {
        
            return new List<string>();
        }

        public async Task<bool> HasPermissionAsync(int roleId, string permission)
        {
            
            return true;
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            if (!await _repository.ExistsAsync(roleId))
                throw new NotFoundException($"Rol con ID {roleId} no encontrado");

            return await _repository.AssignRoleToUserAsync(userId, roleId);
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            return await _repository.RemoveRoleFromUserAsync(userId, roleId);
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersInRoleAsync(int roleId)
        {
            var users = await _repository.GetUsersByRoleAsync(roleId);
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
    }
}