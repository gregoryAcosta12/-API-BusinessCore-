using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Address;

namespace BusinessCore.Application.Interfaces
{
   
    public interface IAddressService
    {
        
        Task<AddressResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<AddressResponseDto>> GetUserAddressesAsync(int userId);
        Task<IEnumerable<AddressResponseDto>> GetActiveUserAddressesAsync(int userId);
        Task<AddressResponseDto> GetDefaultAddressAsync(int userId);
        Task<AddressResponseDto> CreateAsync(AddressCreateDto createDto);
        Task<AddressResponseDto> UpdateAsync(AddressUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        
        Task<bool> SetDefaultAsync(int userId, int addressId);
        Task<bool> HasAddressesAsync(int userId);
        Task<int> GetAddressCountAsync(int userId);
        Task<bool> ValidateAddressAsync(AddressCreateDto addressDto);
    }
}