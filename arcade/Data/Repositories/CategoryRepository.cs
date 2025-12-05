using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data.Repositories
{
    /// <summary>
    /// Repository interface for Category operations
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllActiveAsync();
        Task<IEnumerable<Category>> GetWithProductCountsAsync();
        Task<Category?> GetByNameAsync(string name);
    }

    /// <summary>
    /// Repository implementation for Category operations
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllActiveAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetWithProductCountsAsync()
        {
            return await _dbSet
                .Include(c => c.Products.Where(p => p.IsActive))
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }
    }
}
