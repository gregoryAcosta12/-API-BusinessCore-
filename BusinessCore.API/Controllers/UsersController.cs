using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.User;
using BusinessCore.Application.DTOs.Users;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<UserResponseDto>>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<UserResponseDto>>(users, "Usuarios obtenidos exitosamente"));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<UserResponseDto>>>> GetActive()
        {
            var users = await _userService.GetActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<UserResponseDto>>(users, "Usuarios activos obtenidos exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<UserResponseDto>>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<UserResponseDto>(user, "Usuario obtenido exitosamente"));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<ApiResponseDto<UserResponseDto>>> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            return Ok(new ApiResponseDto<UserResponseDto>(user, "Usuario obtenido exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<UserResponseDto>>>> GetPaged([FromQuery] UserFilterDto filter)
        {
            var result = await _userService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<UserResponseDto>>(result, "Usuarios paginados obtenidos exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<UserResponseDto>>> Create([FromBody] UserCreateDto createDto)
        {
            var user = await _userService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id },
                new ApiResponseDto<UserResponseDto>(user, "Usuario creado exitosamente"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto<UserResponseDto>>> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            return Ok(new ApiResponseDto<UserResponseDto>(user, "Usuario registrado exitosamente"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto<LoginResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(new ApiResponseDto<LoginResponseDto>(result, "Login exitoso"));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponseDto<LoginResponseDto>>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _userService.RefreshTokenAsync(refreshToken);
            return Ok(new ApiResponseDto<LoginResponseDto>(result, "Token refrescado exitosamente"));
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Logout([FromBody] int userId)
        {
            var result = await _userService.LogoutAsync(userId);
            return Ok(new ApiResponseDto<bool>(result, "Logout exitoso"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<UserResponseDto>>> Update(int id, [FromBody] UserUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<UserResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var user = await _userService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<UserResponseDto>(user, "Usuario actualizado exitosamente"));
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await _userService.ChangePasswordAsync(changePasswordDto);
            return Ok(new ApiResponseDto<bool>(result, "Contraseña cambiada exitosamente"));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ResetPassword([FromBody] string email)
        {
            var result = await _userService.ResetPasswordAsync(email);
            return Ok(new ApiResponseDto<bool>(result, "Solicitud de reseteo de contraseña enviada exitosamente"));
        }

        [HttpPost("{userId}/assign-role/{roleId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AssignRole(int userId, int roleId)
        {
            var result = await _userService.AssignRoleAsync(userId, roleId);
            return Ok(new ApiResponseDto<bool>(result, "Rol asignado exitosamente"));
        }

        [HttpPost("{userId}/remove-role/{roleId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveRole(int userId, int roleId)
        {
            var result = await _userService.RemoveRoleAsync(userId, roleId);
            return Ok(new ApiResponseDto<bool>(result, "Rol removido exitosamente"));
        }

        [HttpGet("{userId}/roles")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<string>>>> GetUserRoles(int userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(new ApiResponseDto<IEnumerable<string>>(roles, "Roles del usuario obtenidos exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Usuario eliminado exitosamente"));
        }

        [HttpPost("{userId}/lock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> LockUser(int userId)
        {
            var result = await _userService.LockUserAsync(userId);
            return Ok(new ApiResponseDto<bool>(result, "Usuario bloqueado exitosamente"));
        }

        [HttpPost("{userId}/unlock")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UnlockUser(int userId)
        {
            var result = await _userService.UnlockUserAsync(userId);
            return Ok(new ApiResponseDto<bool>(result, "Usuario desbloqueado exitosamente"));
        }

        [HttpPost("{userId}/confirm-email")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ConfirmEmail(int userId, [FromBody] string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);
            return Ok(new ApiResponseDto<bool>(result, "Email confirmado exitosamente"));
        }

        [HttpPost("resend-confirmation")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ResendConfirmation([FromBody] string email)
        {
            var result = await _userService.ResendConfirmationEmailAsync(email);
            return Ok(new ApiResponseDto<bool>(result, "Email de confirmación reenviado exitosamente"));
        }

        [HttpGet("exists/{email}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ExistsByEmail(string email)
        {
            var result = await _userService.ExistsByEmailAsync(email);
            return Ok(new ApiResponseDto<bool>(result, "Verificación de existencia exitosa"));
        }
    }
}