using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data.Repositories
{
    /// <summary>
    /// Repository interface for Order operations
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<int> GetTodayOrderCountAsync();
        Task<decimal> GetTodayRevenueAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetOrderCountByStatusAsync(string status);
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            int? userId = null,
            string? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }

    /// <summary>
    /// Repository implementation for Order operations
    /// </summary>
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetWithDetailsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _dbSet
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .Where(o => o.Status == "Pending")
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<int> GetTodayOrderCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _dbSet.CountAsync(o => o.OrderDate.Date == today);
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _dbSet
                .Where(o => o.OrderDate.Date == today)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _dbSet
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<int> GetOrderCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(o => o.Status == status);
        }

        public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            int? userId = null,
            string? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }
    }
}
