using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Customer;

namespace BusinessCore.Application.Interfaces
{
  
    public interface ICustomerService
    {
      
        Task<CustomerResponseDto> GetByIdAsync(int id);
        Task<CustomerResponseDto> GetByUserIdAsync(int userId);
        Task<CustomerResponseDto> GetByTaxIdAsync(string taxId);
        Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
        Task<PagedResultDto<CustomerResponseDto>> GetPagedAsync(CustomerFilterDto filter);
        Task<CustomerResponseDto> CreateAsync(CustomerCreateDto createDto);
        Task<CustomerResponseDto> UpdateAsync(CustomerUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        
        Task<IEnumerable<CustomerResponseDto>> GetCustomersWithBalanceAsync();
        Task<decimal> GetTotalCreditBalanceAsync();
        Task<decimal> GetCustomerBalanceAsync(int customerId);

       
        Task<bool> UpdateCreditLimitAsync(int customerId, decimal newLimit);
        Task<bool> AdjustBalanceAsync(int customerId, decimal amount, string reason);
        Task<bool> PayBalanceAsync(int customerId, decimal amount);
    }
}