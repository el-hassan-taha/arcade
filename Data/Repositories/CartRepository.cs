using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data.Repositories
{
    /// <summary>
    /// Repository interface for Cart operations
    /// </summary>
    public interface ICartRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetUserCartAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
        Task<int> GetCartItemCountAsync(int userId);
        Task<decimal> GetCartTotalAsync(int userId);
        Task ClearCartAsync(int userId);
    }

    /// <summary>
    /// Repository implementation for Cart operations
    /// </summary>
    public class CartRepository : Repository<CartItem>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CartItem>> GetUserCartAsync(int userId)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                    .ThenInclude(p => p!.Category)
                .Where(ci => ci.UserId == userId)
                .OrderByDescending(ci => ci.AddedAt)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _dbSet
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Product!.Price * ci.Quantity);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await _dbSet.Where(ci => ci.UserId == userId).ToListAsync();
            _dbSet.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
