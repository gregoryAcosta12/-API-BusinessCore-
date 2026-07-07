using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Address;
using BusinessCore.Application.DTOs.Addresses;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AddressResponseDto> GetByIdAsync(int id)
        {
            var address = await _repository.GetByIdAsync(id);
            if (address == null)
                throw new NotFoundException($"Dirección con ID {id} no encontrada");

            return _mapper.Map<AddressResponseDto>(address);
        }

        public async Task<IEnumerable<AddressResponseDto>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _repository.GetUserAddressesAsync(userId);
            return _mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
        }

        public async Task<IEnumerable<AddressResponseDto>> GetActiveUserAddressesAsync(int userId)
        {
            var addresses = await _repository.GetActiveUserAddressesAsync(userId);
            return _mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
        }

        public async Task<AddressResponseDto> GetDefaultAddressAsync(int userId)
        {
            var address = await _repository.GetDefaultAddressAsync(userId);
            if (address == null)
                throw new NotFoundException($"No se encontró dirección predeterminada para el usuario {userId}");

            return _mapper.Map<AddressResponseDto>(address);
        }

        public async Task<AddressResponseDto> CreateAsync(AddressCreateDto createDto)
        {
            var address = _mapper.Map<Address>(createDto);
            address.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(address);
            return _mapper.Map<AddressResponseDto>(created);
        }

        public async Task<AddressResponseDto> UpdateAsync(AddressUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Dirección con ID {updateDto.Id} no encontrada");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<AddressResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Dirección con ID {id} no encontrada");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> SetDefaultAsync(int userId, int addressId)
        {
            if (!await _repository.ExistsAsync(addressId))
                throw new NotFoundException($"Dirección con ID {addressId} no encontrada");

            return await _repository.SetDefaultAsync(userId, addressId);
        }

        public async Task<bool> HasAddressesAsync(int userId)
        {
            return await _repository.HasAddressesAsync(userId);
        }

        public async Task<int> GetAddressCountAsync(int userId)
        {
            var addresses = await _repository.GetUserAddressesAsync(userId);
            return addresses.Count();
        }
    }
}