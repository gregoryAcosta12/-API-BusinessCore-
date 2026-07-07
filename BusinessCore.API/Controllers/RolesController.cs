using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Roles;
using BusinessCore.Application.DTOs.User;
using BusinessCore.Application.DTOs.Users;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<RoleResponseDto>>>> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<RoleResponseDto>>(roles, "Roles obtenidos exitosamente"));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<RoleResponseDto>>>> GetActive()
        {
            var roles = await _roleService.GetActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<RoleResponseDto>>(roles, "Roles activos obtenidos exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<RoleResponseDto>(role, "Rol obtenido exitosamente"));
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> GetByName(string name)
        {
            var role = await _roleService.GetByNameAsync(name);
            return Ok(new ApiResponseDto<RoleResponseDto>(role, "Rol obtenido exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<RoleResponseDto>>>> GetPaged([FromQuery] RoleFilterDto filter)
        {
            var result = await _roleService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<RoleResponseDto>>(result, "Roles paginados obtenidos exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> Create([FromBody] RoleCreateDto createDto)
        {
            var role = await _roleService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = role.Id },
                new ApiResponseDto<RoleResponseDto>(role, "Rol creado exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> Update(int id, [FromBody] RoleUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<RoleResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var role = await _roleService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<RoleResponseDto>(role, "Rol actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _roleService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Rol eliminado exitosamente"));
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AssignPermission(int roleId, int permissionId)
        {
            var result = await _roleService.AssignPermissionAsync(roleId, permissionId);
            return Ok(new ApiResponseDto<bool>(result, "Permiso asignado al rol exitosamente"));
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemovePermission(int roleId, int permissionId)
        {
            var result = await _roleService.RemovePermissionAsync(roleId, permissionId);
            return Ok(new ApiResponseDto<bool>(result, "Permiso removido del rol exitosamente"));
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<string>>>> GetRolePermissions(int roleId)
        {
            var permissions = await _roleService.GetRolePermissionsAsync(roleId);
            return Ok(new ApiResponseDto<IEnumerable<string>>(permissions, "Permisos del rol obtenidos exitosamente"));
        }

        [HttpGet("{roleId}/has-permission/{permission}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> HasPermission(int roleId, string permission)
        {
            var result = await _roleService.HasPermissionAsync(roleId, permission);
            return Ok(new ApiResponseDto<bool>(result, "Verificación de permiso exitosa"));
        }

        [HttpGet("{roleId}/users")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<UserResponseDto>>>> GetUsersInRole(int roleId)
        {
            var users = await _roleService.GetUsersInRoleAsync(roleId);
            return Ok(new ApiResponseDto<IEnumerable<UserResponseDto>>(users, "Usuarios del rol obtenidos exitosamente"));
        }
    }
}