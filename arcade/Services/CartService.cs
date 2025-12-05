using Arcade.Data.Repositories;
using Arcade.Models;

namespace Arcade.Services
{
    /// <summary>
    /// Service interface for Cart operations
    /// </summary>
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
        Task<decimal> GetCartTotalAsync(int userId);
        Task<(bool Success, string Message)> AddToCartAsync(int userId, int productId, int quantity = 1);
        Task<(bool Success, string Message)> UpdateQuantityAsync(int userId, int productId, int quantity);
        Task<(bool Success, string Message)> RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
        Task<bool> ValidateCartStockAsync(int userId);
    }

    /// <summary>
    /// Service implementation for Cart operations
    /// </summary>
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CartItem>> GetCartAsync(int userId)
        {
            return await _cartRepository.GetUserCartAsync(userId);
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _cartRepository.GetCartItemCountAsync(userId);
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            return await _cartRepository.GetCartTotalAsync(userId);
        }

        public async Task<(bool Success, string Message)> AddToCartAsync(int userId, int productId, int quantity = 1)
        {
            if (quantity <= 0)
                return (false, "Quantity must be at least 1.");

            // Get product
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
                return (false, "Product not found.");

            // Check stock
            var existingItem = await _cartRepository.GetCartItemAsync(userId, productId);
            var currentQuantity = existingItem?.Quantity ?? 0;
            var requestedTotal = currentQuantity + quantity;

            if (product.StockQuantity < requestedTotal)
            {
                if (product.StockQuantity == 0)
                    return (false, "This product is out of stock.");
                return (false, $"Only {product.StockQuantity} items available in stock.");
            }

            if (existingItem != null)
            {
                // Update existing cart item
                existingItem.Quantity = requestedTotal;
                existingItem.UpdatedAt = DateTime.UtcNow;
                _cartRepository.Update(existingItem);
            }
            else
            {
                // Add new cart item
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(cartItem);
            }

            await _cartRepository.SaveChangesAsync();
            return (true, "Product added to cart successfully!");
        }

        public async Task<(bool Success, string Message)> UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveFromCartAsync(userId, productId);
            }

            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
            if (cartItem == null)
                return (false, "Item not found in cart.");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return (false, "Product not found.");

            if (quantity > product.StockQuantity)
                return (false, $"Only {product.StockQuantity} items available in stock.");

            cartItem.Quantity = quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            _cartRepository.Update(cartItem);
            await _cartRepository.SaveChangesAsync();

            return (true, "Cart updated successfully.");
        }

        public async Task<(bool Success, string Message)> RemoveFromCartAsync(int userId, int productId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
            if (cartItem == null)
                return (false, "Item not found in cart.");

            _cartRepository.Remove(cartItem);
            await _cartRepository.SaveChangesAsync();

            return (true, "Item removed from cart.");
        }

        public async Task ClearCartAsync(int userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }

        public async Task<bool> ValidateCartStockAsync(int userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            foreach (var item in cartItems)
            {
                if (item.Product == null || item.Product.StockQuantity < item.Quantity)
                    return false;
            }

            return true;
        }
    }
}
