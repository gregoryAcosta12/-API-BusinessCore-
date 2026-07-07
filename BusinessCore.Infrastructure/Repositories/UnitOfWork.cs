using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(
            ApplicationDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IInventoryRepository inventoryRepository,
            IReviewRepository reviewRepository,
            IPaymentRepository paymentRepository,
            IAddressRepository addressRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
            Users = userRepository;
            Orders = orderRepository;
            Inventory = inventoryRepository;
            Reviews = reviewRepository;
            Payments = paymentRepository;
            Addresses = addressRepository;
        }

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IUserRepository Users { get; }
        public IOrderRepository Orders { get; }
        public IInventoryRepository Inventory { get; }
        public IReviewRepository Reviews { get; }
        public IPaymentRepository Payments { get; }
        public IAddressRepository Addresses { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction?.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction?.RollbackAsync();
            _transaction?.Dispose();
            _transaction = null;
        }

        public async Task<bool> HasChangesAsync()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}