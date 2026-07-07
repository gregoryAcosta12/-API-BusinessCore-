using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Customer;
using BusinessCore.Application.DTOs.Customers;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponseDto> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException($"Cliente con ID {id} no encontrado");

            return _mapper.Map<CustomerResponseDto>(customer);
        }

        public async Task<CustomerResponseDto> GetByUserIdAsync(int userId)
        {
            var customer = await _repository.GetByUserIdAsync(userId);
            if (customer == null)
                throw new NotFoundException($"Cliente con UserId {userId} no encontrado");

            return _mapper.Map<CustomerResponseDto>(customer);
        }

        public async Task<CustomerResponseDto> GetByTaxIdAsync(string taxId)
        {
            var customer = await _repository.GetByTaxIdAsync(taxId);
            if (customer == null)
                throw new NotFoundException($"Cliente con TaxId {taxId} no encontrado");

            return _mapper.Map<CustomerResponseDto>(customer);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
        }

        public async Task<PagedResultDto<CustomerResponseDto>> GetPagedAsync(CustomerFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var customers = await query;

            if (!string.IsNullOrEmpty(filter.CompanyName))
                customers = customers.Where(c => c.CompanyName.Contains(filter.CompanyName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.TaxId))
                customers = customers.Where(c => c.TaxId == filter.TaxId);

            if (!string.IsNullOrEmpty(filter.BusinessType))
                customers = customers.Where(c => c.BusinessType == filter.BusinessType);

            if (filter.IsActive.HasValue)
                customers = customers.Where(c => c.IsActive == filter.IsActive);

            if (filter.HasBalance.HasValue && filter.HasBalance.Value)
                customers = customers.Where(c => c.CurrentBalance > 0);

            var totalCount = customers.Count();
            var items = customers
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<CustomerResponseDto>
            {
                Items = _mapper.Map<IEnumerable<CustomerResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto createDto)
        {
            if (await _repository.ExistsByTaxIdAsync(createDto.TaxId))
                throw new ConflictException($"El TaxId {createDto.TaxId} ya está registrado");

            var customer = _mapper.Map<Customer>(createDto);
            customer.CreatedAt = DateTime.UtcNow;
            customer.CurrentBalance = 0;

            var created = await _repository.CreateAsync(customer);
            return _mapper.Map<CustomerResponseDto>(created);
        }

        public async Task<CustomerResponseDto> UpdateAsync(CustomerUpdateDto updateDto)
        {
            var existing = await _repository.GetByIdAsync(updateDto.Id);
            if (existing == null)
                throw new NotFoundException($"Cliente con ID {updateDto.Id} no encontrado");

            if (existing.TaxId != updateDto.TaxId && await _repository.ExistsByTaxIdAsync(updateDto.TaxId))
                throw new ConflictException($"El TaxId {updateDto.TaxId} ya está registrado");

            _mapper.Map(updateDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<CustomerResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Cliente con ID {id} no encontrado");

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetCustomersWithBalanceAsync()
        {
            var customers = await _repository.GetCustomersWithBalanceAsync();
            return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
        }

        public async Task<decimal> GetTotalCreditBalanceAsync()
        {
            return await _repository.GetTotalCreditBalanceAsync();
        }

        public async Task<decimal> GetCustomerBalanceAsync(int customerId)
        {
            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
                throw new NotFoundException($"Cliente con ID {customerId} no encontrado");

            return customer.CurrentBalance;
        }

        public async Task<bool> UpdateCreditLimitAsync(int customerId, decimal newLimit)
        {
            if (newLimit < 0)
                throw new BadRequestException("El límite de crédito no puede ser negativo");

            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
                throw new NotFoundException($"Cliente con ID {customerId} no encontrado");

            customer.CreditLimit = newLimit;
            customer.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(customer);

            return true;
        }

        public async Task<bool> AdjustBalanceAsync(int customerId, decimal amount, string reason)
        {
            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
                throw new NotFoundException($"Cliente con ID {customerId} no encontrado");

            customer.CurrentBalance += amount;
            customer.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(customer);

            return true;
        }

        public async Task<bool> PayBalanceAsync(int customerId, decimal amount)
        {
            if (amount <= 0)
                throw new BadRequestException("El monto a pagar debe ser mayor a 0");

            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
                throw new NotFoundException($"Cliente con ID {customerId} no encontrado");

            if (amount > customer.CurrentBalance)
                throw new BusinessException("El monto a pagar excede el saldo actual");

            customer.CurrentBalance -= amount;
            customer.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(customer);

            return true;
        }
    }
}