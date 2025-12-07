using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data.Repositories
{
    /// <summary>
    /// Repository interface for User operations
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetWithCartAsync(int userId);
        Task<User?> GetWithOrdersAsync(int userId);
        Task<int> GetCustomerCountAsync();
    }

    /// <summary>
    /// Repository implementation for User operations
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetWithCartAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetWithOrdersAsync(int userId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<int> GetCustomerCountAsync()
        {
            return await _dbSet.AsNoTracking().CountAsync(u => u.Role == "Customer" && u.IsActive);
        }
    }
}
