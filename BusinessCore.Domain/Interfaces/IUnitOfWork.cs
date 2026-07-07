using System;
using System.Threading.Tasks;

namespace BusinessCore.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        IInventoryRepository Inventory { get; }
        IReviewRepository Reviews { get; }
        IPaymentRepository Payments { get; }
        IAddressRepository Addresses { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> HasChangesAsync();
    }
}