using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> GetByUserIdAsync(int userId);
        Task<Customer> GetByTaxIdAsync(string taxId);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByTaxIdAsync(string taxId);
        Task<IEnumerable<Customer>> GetCustomersWithBalanceAsync();
        Task<decimal> GetTotalCreditBalanceAsync();
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Orders)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Orders)
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Customer> GetByUserIdAsync(int userId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
        }

        public async Task<Customer> GetByTaxIdAsync(string taxId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.TaxId == taxId && c.IsActive);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            customer.CurrentBalance = 0;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer == null)
                return false;

            customer.IsActive = false;
            customer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<bool> ExistsByTaxIdAsync(string taxId)
        {
            return await _context.Customers.AnyAsync(c => c.TaxId == taxId && c.IsActive);
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithBalanceAsync()
        {
            return await _context.Customers
                .Include(c => c.User)
                .Where(c => c.CurrentBalance > 0 && c.IsActive)
                .OrderByDescending(c => c.CurrentBalance)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalCreditBalanceAsync()
        {
            return await _context.Customers
                .Where(c => c.IsActive)
                .SumAsync(c => c.CurrentBalance);
        }
    }
}