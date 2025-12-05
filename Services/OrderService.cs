using Arcade.Data.Repositories;
using Arcade.Models;

namespace Arcade.Services
{
    /// <summary>
    /// Service interface for Order operations
    /// </summary>
    public interface IOrderService
    {
        Task<Order?> GetByIdAsync(int orderId);
        Task<Order?> GetWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<(IEnumerable<Order> Orders, int TotalCount, int TotalPages)> GetPagedAsync(
            int page, int pageSize, int? userId = null, string? status = null);
        Task<(bool Success, string Message, Order? Order)> CreateOrderAsync(int userId, string street, string city, string? notes = null);
        Task<bool> UpdateStatusAsync(int orderId, string status);
        Task<int> GetPendingOrderCountAsync();
        Task<int> GetTodayOrderCountAsync();
        Task<decimal> GetTodayRevenueAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetOrderCountByStatusAsync(string status);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
    }

    /// <summary>
    /// Service implementation for Order operations
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<Order?> GetWithDetailsAsync(int orderId)
        {
            return await _orderRepository.GetWithDetailsAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetUserOrdersAsync(userId);
        }

        public async Task<(IEnumerable<Order> Orders, int TotalCount, int TotalPages)> GetPagedAsync(
            int page, int pageSize, int? userId = null, string? status = null)
        {
            var (orders, totalCount) = await _orderRepository.GetPagedAsync(page, pageSize, userId, status);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return (orders, totalCount, totalPages);
        }

        public async Task<(bool Success, string Message, Order? Order)> CreateOrderAsync(
            int userId, string street, string city, string? notes = null)
        {
            // Get cart items
            var cartItems = (await _cartRepository.GetUserCartAsync(userId)).ToList();

            if (!cartItems.Any())
                return (false, "Your cart is empty.", null);

            // Validate stock
            foreach (var item in cartItems)
            {
                if (item.Product == null || item.Product.StockQuantity < item.Quantity)
                {
                    return (false, $"Insufficient stock for {item.Product?.ProductName ?? "Unknown product"}", null);
                }
            }

            // Calculate total
            decimal totalAmount = cartItems.Sum(ci => ci.Product!.Price * ci.Quantity);

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                Street = street,
                City = city,
                Notes = notes
            };

            // Create order details
            var orderDetails = new List<OrderDetail>();
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product!.Price,
                    ProductName = item.Product.ProductName,
                    ProductImageUrl = item.Product.ImageUrl
                };
                orderDetails.Add(orderDetail);

                // Update stock
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    _productRepository.Update(product);
                }
            }

            order.OrderDetails = orderDetails;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // Clear cart
            await _cartRepository.ClearCartAsync(userId);

            return (true, "Order placed successfully!", order);
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            var validStatuses = new[] { "Pending", "Processing", "Shipped", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status))
                return false;

            order.Status = status;

            if (status == "Shipped" && !order.ShippedDate.HasValue)
                order.ShippedDate = DateTime.UtcNow;

            if (status == "Completed" && !order.DeliveredDate.HasValue)
                order.DeliveredDate = DateTime.UtcNow;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetPendingOrderCountAsync()
        {
            return await _orderRepository.CountAsync(o => o.Status == "Pending");
        }

        public async Task<int> GetTodayOrderCountAsync()
        {
            return await _orderRepository.GetTodayOrderCountAsync();
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            return await _orderRepository.GetTodayRevenueAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _orderRepository.GetTotalRevenueAsync();
        }

        public async Task<int> GetOrderCountByStatusAsync(string status)
        {
            return await _orderRepository.GetOrderCountByStatusAsync(status);
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _orderRepository.GetRecentOrdersAsync(count);
        }
    }
}
