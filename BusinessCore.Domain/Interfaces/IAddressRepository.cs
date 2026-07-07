using BusinessCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetByIdAsync(int id);
        Task<IEnumerable<Address>> GetUserAddressesAsync(int userId);
        Task<IEnumerable<Address>> GetActiveUserAddressesAsync(int userId);
        Task<Address> GetDefaultAddressAsync(int userId);
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task<bool> DeleteAsync(int id);
        Task<bool> SetDefaultAsync(int userId, int addressId);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasAddressesAsync(int userId);
    }
}