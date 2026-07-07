using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Address;
using BusinessCore.Application.DTOs.Addresses;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<AddressResponseDto>>> GetById(int id)
        {
            var address = await _addressService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<AddressResponseDto>(address, "Dirección obtenida exitosamente"));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<AddressResponseDto>>>> GetUserAddresses(int userId)
        {
            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(new ApiResponseDto<IEnumerable<AddressResponseDto>>(addresses, "Direcciones del usuario obtenidas exitosamente"));
        }

        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<AddressResponseDto>>>> GetActiveUserAddresses(int userId)
        {
            var addresses = await _addressService.GetActiveUserAddressesAsync(userId);
            return Ok(new ApiResponseDto<IEnumerable<AddressResponseDto>>(addresses, "Direcciones activas del usuario obtenidas exitosamente"));
        }

        [HttpGet("user/{userId}/default")]
        public async Task<ActionResult<ApiResponseDto<AddressResponseDto>>> GetDefaultAddress(int userId)
        {
            var address = await _addressService.GetDefaultAddressAsync(userId);
            return Ok(new ApiResponseDto<AddressResponseDto>(address, "Dirección predeterminada obtenida exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<AddressResponseDto>>> Create([FromBody] AddressCreateDto createDto)
        {
            var address = await _addressService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = address.Id },
                new ApiResponseDto<AddressResponseDto>(address, "Dirección creada exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<AddressResponseDto>>> Update(int id, [FromBody] AddressUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<AddressResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var address = await _addressService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<AddressResponseDto>(address, "Dirección actualizada exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _addressService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Dirección eliminada exitosamente"));
        }

        [HttpPost("user/{userId}/set-default/{addressId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> SetDefault(int userId, int addressId)
        {
            var result = await _addressService.SetDefaultAsync(userId, addressId);
            return Ok(new ApiResponseDto<bool>(result, "Dirección predeterminada establecida exitosamente"));
        }

        [HttpGet("user/{userId}/has-addresses")]
        public async Task<ActionResult<ApiResponseDto<bool>>> HasAddresses(int userId)
        {
            var result = await _addressService.HasAddressesAsync(userId);
            return Ok(new ApiResponseDto<bool>(result, "Verificación de direcciones exitosa"));
        }

        [HttpGet("user/{userId}/count")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetAddressCount(int userId)
        {
            var count = await _addressService.GetAddressCountAsync(userId);
            return Ok(new ApiResponseDto<int>(count, "Conteo de direcciones obtenido exitosamente"));
        }
    }
}