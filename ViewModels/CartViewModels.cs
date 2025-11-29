using Arcade.Models;

namespace Arcade.ViewModels
{
    /// <summary>
    /// ViewModel for shopping cart display
    /// </summary>
    public class CartViewModel
    {
        public IEnumerable<CartItemViewModel> Items { get; set; } = Enumerable.Empty<CartItemViewModel>();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
        public int TotalItems { get; set; }
        public bool HasItems => ItemCount > 0;
    }

    /// <summary>
    /// ViewModel for individual cart item
    /// </summary>
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
        public decimal Total => UnitPrice * Quantity;
        public int StockQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public bool IsInStock => StockQuantity > 0;
        public bool HasInsufficientStock => Quantity > StockQuantity;
    }

    /// <summary>
    /// ViewModel for adding item to cart
    /// </summary>
    public class AddToCartViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    /// <summary>
    /// ViewModel for updating cart item quantity
    /// </summary>
    public class UpdateCartItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Response model for cart operations
    /// </summary>
    public class CartOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CartItemCount { get; set; }
        public decimal CartTotal { get; set; }
    }
}
