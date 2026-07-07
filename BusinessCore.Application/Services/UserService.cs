using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.User;
using BusinessCore.Application.DTOs.Users;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessCore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {id} no encontrado");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new NotFoundException($"Usuario con email {email} no encontrado");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<IEnumerable<UserResponseDto>> GetActiveAsync()
        {
            var users = await _repository.GetActiveAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<PagedResultDto<UserResponseDto>> GetPagedAsync(UserFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var users = await query;

            if (!string.IsNullOrEmpty(filter.Email))
                users = users.Where(u => u.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.FirstName))
                users = users.Where(u => u.FirstName.Contains(filter.FirstName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.LastName))
                users = users.Where(u => u.LastName.Contains(filter.LastName, StringComparison.OrdinalIgnoreCase));

            if (filter.IsActive.HasValue)
                users = users.Where(u => u.IsActive == filter.IsActive);

            var totalCount = users.Count();
            var items = users
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<UserResponseDto>
            {
                Items = _mapper.Map<IEnumerable<UserResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto createDto)
        {
            if (await _repository.ExistsByEmailAsync(createDto.Email))
                throw new ConflictException($"El email {createDto.Email} ya está registrado");

            var user = _mapper.Map<User>(createDto);
            user.PasswordHash = HashPassword(createDto.Password);
            user.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(user);

            if (createDto.RoleIds != null)
            {
                foreach (var roleId in createDto.RoleIds)
                {
                    await _roleRepository.AssignRoleToUserAsync(created.Id, roleId);
                }
            }

            return _mapper.Map<UserResponseDto>(created);
        }

        public async Task<UserResponseDto> UpdateAsync(UserUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Usuario con ID {updateDto.Id} no encontrado");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<UserResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Usuario con ID {id} no encontrado");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _repository.ExistsByEmailAsync(email);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _repository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedException("Credenciales inválidas");

            if (!user.IsActive)
                throw new UnauthorizedException("Usuario inactivo");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Email no confirmado");

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            await _repository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));
            await _repository.UpdateLastLoginAsync(user.Id);

            return new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _repository.ExistsByEmailAsync(registerDto.Email))
                throw new ConflictException($"El email {registerDto.Email} ya está registrado");

            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new BadRequestException("Las contraseñas no coinciden");

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = HashPassword(registerDto.Password);
            user.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(user);

            // Asignar rol de Customer por defecto
            var customerRole = await _roleRepository.GetByNameAsync("Customer");
            if (customerRole != null)
                await _roleRepository.AssignRoleToUserAsync(created.Id, customerRole.Id);

            return _mapper.Map<UserResponseDto>(created);
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _repository.GetByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedException("Token de refresco inválido o expirado");

            var token = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await _repository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                AccessToken = token,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            return await _repository.UpdateRefreshTokenAsync(userId, null, null);
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _repository.GetByIdAsync(changePasswordDto.UserId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {changePasswordDto.UserId} no encontrado");

            if (!VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                throw new BadRequestException("Contraseña actual incorrecta");

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
                throw new BadRequestException("Las nuevas contraseñas no coinciden");

            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new NotFoundException($"Usuario con email {email} no encontrado");

            // Aquí iría el envío de email con el link de reseteo
            return true;
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            if (!await _repository.ExistsAsync(userId))
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");

            if (!await _roleRepository.ExistsAsync(roleId))
                throw new NotFoundException($"Rol con ID {roleId} no encontrado");

            return await _roleRepository.AssignRoleToUserAsync(userId, roleId);
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            return await _roleRepository.RemoveRoleFromUserAsync(userId, roleId);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");

            return user.UserRoles?.Select(ur => ur.Role.Name) ?? new List<string>();
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                return false;

            return user.UserRoles?.Any(ur => ur.Role.Name == roleName) ?? false;
        }

        public async Task<bool> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");

            user.EmailConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ResendConfirmationEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new NotFoundException($"Usuario con email {email} no encontrado");

            // Aquí iría el envío de email de confirmación
            return true;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);

            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "supersecretkey1234567890");
            var roles = user.UserRoles?.Select(ur => ur.Role.Name) ?? new List<string>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"] ?? "BusinessCore",
                Audience = _configuration["Jwt:Audience"] ?? "BusinessCore",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}