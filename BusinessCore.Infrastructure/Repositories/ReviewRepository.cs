using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Interfaces;
using BusinessCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCore.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId && r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetPendingReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => r.Status == ReviewStatus.Pending)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review> CreateAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            review.Status = ReviewStatus.Pending;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Actualizar rating promedio del producto
            await UpdateProductAverageRating(review.ProductId);

            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            review.UpdatedAt = DateTime.UtcNow;
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Actualizar rating promedio del producto
            await UpdateProductAverageRating(review.ProductId);

            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await GetByIdAsync(id);
            if (review == null)
                return false;

            review.IsActive = false;
            review.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Actualizar rating promedio del producto
            await UpdateProductAverageRating(review.ProductId);

            return true;
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsActive && r.Status == ReviewStatus.Approved)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }

        public async Task<int> GetReviewCountAsync(int productId)
        {
            return await _context.Reviews
                .CountAsync(r => r.ProductId == productId && r.IsActive && r.Status == ReviewStatus.Approved);
        }

        public async Task<IEnumerable<Review>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetProductReviewsPagedAsync(int productId, int pageNumber, int pageSize)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId && r.IsActive && r.Status == ReviewStatus.Approved)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private async Task UpdateProductAverageRating(int productId)
        {
            var average = await GetAverageRatingAsync(productId);
            var count = await GetReviewCountAsync(productId);

            // Aquí podrías actualizar el producto con el nuevo rating
            // Si tu entidad Product tiene propiedades AverageRating y ReviewCount
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                // product.AverageRating = average; // Si tienes esta propiedad
                // product.ReviewCount = count; // Si tienes esta propiedad
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}