using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetActiveUserAddressesAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Address> GetDefaultAddressAsync(int userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault && a.IsActive);
        }

        public async Task<Address> CreateAsync(Address address)
        {
            address.CreatedAt = DateTime.UtcNow;

            // Si es la dirección predeterminada, quitar el default de las otras
            if (address.IsDefault)
            {
                await RemoveDefaultFromOtherAddresses(address.UserId);
            }

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            address.UpdatedAt = DateTime.UtcNow;

            // Si es la dirección predeterminada, quitar el default de las otras
            if (address.IsDefault)
            {
                await RemoveDefaultFromOtherAddresses(address.UserId, address.Id);
            }

            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await GetByIdAsync(id);
            if (address == null)
                return false;

            address.IsActive = false;
            address.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetDefaultAsync(int userId, int addressId)
        {
            await RemoveDefaultFromOtherAddresses(userId, addressId);

            var address = await GetByIdAsync(addressId);
            if (address == null)
                return false;

            address.IsDefault = true;
            address.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Addresses.AnyAsync(a => a.Id == id && a.IsActive);
        }

        public async Task<bool> HasAddressesAsync(int userId)
        {
            return await _context.Addresses.AnyAsync(a => a.UserId == userId && a.IsActive);
        }

        private async Task RemoveDefaultFromOtherAddresses(int userId, int? excludeAddressId = null)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault && a.Id != excludeAddressId)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = false;
                address.UpdatedAt = DateTime.UtcNow;
                _context.Entry(address).State = EntityState.Modified;
            }

            if (addresses.Any())
                await _context.SaveChangesAsync();
        }
    }
}