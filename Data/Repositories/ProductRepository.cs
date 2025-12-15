using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data.Repositories
{
    /// <summary>
    /// Repository interface for Product operations
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetWithCategoryAsync(int productId);
        Task<IEnumerable<Product>> GetAllWithCategoriesAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10);
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStockOnly = null,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false);
        Task<bool> HasOrderDetailsAsync(int productId);
    }

    /// <summary>
    /// Repository implementation for Product operations
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetWithCategoryAsync(int productId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoriesAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.IsFeatured && p.StockQuantity > 0)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower().Trim();
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive &&
                    (p.ProductName.ToLower().Contains(term) ||
                     (p.Description != null && p.Description.ToLower().Contains(term)) ||
                     (p.Brand != null && p.Brand.ToLower().Contains(term))))
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStockOnly = null,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false)
        {
            var query = _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            // Apply filters
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (inStockOnly == true)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower().Trim();
                query = query.Where(p =>
                    p.ProductName.ToLower().Contains(term) ||
                    (p.Description != null && p.Description.ToLower().Contains(term)) ||
                    (p.Brand != null && p.Brand.ToLower().Contains(term)));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "price" => sortDescending
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),
                "name" => sortDescending
                    ? query.OrderByDescending(p => p.ProductName)
                    : query.OrderBy(p => p.ProductName),
                "stock" => sortDescending
                    ? query.OrderByDescending(p => p.StockQuantity)
                    : query.OrderBy(p => p.StockQuantity),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.ProductName)
            };

            // Apply pagination
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<bool> HasOrderDetailsAsync(int productId)
        {
            return await _context.OrderDetails.AnyAsync(od => od.ProductId == productId);
        }
    }
}
