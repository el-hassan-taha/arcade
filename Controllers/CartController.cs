using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Arcade.Services;
using Arcade.ViewModels;

namespace Arcade.Controllers
{
    /// <summary>
    /// Controller for shopping cart operations
    /// </summary>
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService,
            IProductService productService,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Displays the shopping cart
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);

            var model = new CartViewModel
            {
                Items = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ImageUrl = ci.Product?.ImageUrl,
                    CategoryName = ci.Product?.Category?.CategoryName,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    StockQuantity = ci.Product?.StockQuantity ?? 0,
                    MaxQuantity = ci.Product?.StockQuantity ?? 0
                }),
                ItemCount = cartItems.Sum(ci => ci.Quantity),
                TotalItems = cartItems.Sum(ci => ci.Quantity)
            };

            model.Subtotal = model.Items.Sum(i => i.Subtotal);
            model.Shipping = model.Subtotal >= 2500 ? 0 : 100; // Free shipping over 2500 EGP
            model.Tax = model.Subtotal * 0.14m; // 14% VAT (Egypt)
            model.Total = model.Subtotal + model.Shipping + model.Tax;

            return View(model);
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = GetCurrentUserId();
            var (success, message) = await _cartService.AddToCartAsync(userId, productId, quantity);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var cartCount = await _cartService.GetCartItemCountAsync(userId);
                var cartTotal = await _cartService.GetCartTotalAsync(userId);

                return Json(new CartOperationResult
                {
                    Success = success,
                    Message = message,
                    CartItemCount = cartCount,
                    CartTotal = cartTotal
                });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates cart item quantity
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var userId = GetCurrentUserId();
            var (success, message) = await _cartService.UpdateQuantityAsync(userId, productId, quantity);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var cartItems = await _cartService.GetCartAsync(userId);
                var subtotal = cartItems.Sum(ci => ci.Subtotal);
                var shipping = subtotal >= 2500 ? 0 : 100m;
                var tax = subtotal * 0.14m;
                var total = subtotal + shipping + tax;
                var itemCount = cartItems.Sum(ci => ci.Quantity);

                var item = cartItems.FirstOrDefault(ci => ci.ProductId == productId);

                return Json(new
                {
                    success,
                    message,
                    cartItemCount = itemCount,
                    cartTotal = total,
                    itemSubtotal = item?.Subtotal ?? 0,
                    subtotal,
                    shipping,
                    tax,
                    total
                });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes an item from the cart
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetCurrentUserId();
            var (success, message) = await _cartService.RemoveFromCartAsync(userId, productId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var cartItems = await _cartService.GetCartAsync(userId);
                var subtotal = cartItems.Sum(ci => ci.Subtotal);
                var shipping = subtotal >= 2500 ? 0 : 100m;
                var tax = subtotal * 0.14m;
                var total = subtotal + shipping + tax;
                var itemCount = cartItems.Sum(ci => ci.Quantity);

                return Json(new
                {
                    success,
                    message,
                    cartItemCount = itemCount,
                    cartTotal = total,
                    subtotal,
                    shipping,
                    tax,
                    total
                });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Clears all items from the cart
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var userId = GetCurrentUserId();
            await _cartService.ClearCartAsync(userId);

            TempData["SuccessMessage"] = "Cart cleared successfully.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the cart item count (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Json(new { count = 0 });
            }

            var userId = GetCurrentUserId();
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Json(new { count });
        }

        /// <summary>
        /// Mini cart view for dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> MiniCart()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);

            var model = new CartViewModel
            {
                Items = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ImageUrl = ci.Product?.ImageUrl,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).Take(3),
                ItemCount = cartItems.Sum(ci => ci.Quantity)
            };

            model.Total = cartItems.Sum(ci => ci.Subtotal);

            return PartialView("_MiniCart", model);
        }

        /// <summary>
        /// Gets the current user ID from claims
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
        }
    }
}
