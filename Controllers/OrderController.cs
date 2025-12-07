using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Arcade.Services;
using Arcade.ViewModels;

namespace Arcade.Controllers
{
    /// <summary>
    /// Controller for checkout and order operations
    /// </summary>
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            ICartService cartService,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Checkout page with shipping form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Get user info for pre-filling
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var nameParts = userName?.Split(' ', 2) ?? Array.Empty<string>();

            var items = cartItems.Select(ci => new CartItemViewModel
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.ProductName ?? "Unknown",
                ImageUrl = ci.Product?.ImageUrl,
                UnitPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                StockQuantity = ci.Product?.StockQuantity ?? 0
            }).ToList();

            var subtotal = items.Sum(i => i.Subtotal);
            var shipping = subtotal >= 2500 ? 0 : 100; // Free shipping over 2500 EGP
            var tax = subtotal * 0.14m; // 14% VAT (Egypt)

            var model = new CheckoutViewModel
            {
                Items = items,
                Subtotal = subtotal,
                Shipping = shipping,
                Tax = tax,
                Total = subtotal + shipping + tax,
                FirstName = nameParts.Length > 0 ? nameParts[0] : "",
                LastName = nameParts.Length > 1 ? nameParts[1] : "",
                Email = userEmail ?? ""
            };

            return View(model);
        }

        /// <summary>
        /// Checkout Step 2: Delivery Address
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Address()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Try to get saved address from session
            var model = new CheckoutAddressViewModel();
            if (HttpContext.Session.GetString("CheckoutStreet") != null)
            {
                model.Street = HttpContext.Session.GetString("CheckoutStreet") ?? "";
                model.City = HttpContext.Session.GetString("CheckoutCity") ?? "";
                model.State = HttpContext.Session.GetString("CheckoutState");
                model.PostalCode = HttpContext.Session.GetString("CheckoutPostalCode");
                model.Notes = HttpContext.Session.GetString("CheckoutNotes");
            }

            return View(model);
        }

        /// <summary>
        /// Processes delivery address
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Address(CheckoutAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save address to session
            HttpContext.Session.SetString("CheckoutStreet", model.Street);
            HttpContext.Session.SetString("CheckoutCity", model.City);
            HttpContext.Session.SetString("CheckoutState", model.State ?? "");
            HttpContext.Session.SetString("CheckoutPostalCode", model.PostalCode ?? "");
            HttpContext.Session.SetString("CheckoutNotes", model.Notes ?? "");

            return RedirectToAction("Confirm");
        }

        /// <summary>
        /// Checkout Step 3: Confirm Order
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Confirm()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Get address from session
            var street = HttpContext.Session.GetString("CheckoutStreet");
            var city = HttpContext.Session.GetString("CheckoutCity");

            if (string.IsNullOrEmpty(street) || string.IsNullOrEmpty(city))
            {
                TempData["ErrorMessage"] = "Please enter your delivery address.";
                return RedirectToAction("Address");
            }

            var model = new CheckoutConfirmViewModel
            {
                CartItems = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ImageUrl = ci.Product?.ImageUrl,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }),
                DeliveryAddress = new CheckoutAddressViewModel
                {
                    Street = street,
                    City = city,
                    State = HttpContext.Session.GetString("CheckoutState"),
                    PostalCode = HttpContext.Session.GetString("CheckoutPostalCode"),
                    Notes = HttpContext.Session.GetString("CheckoutNotes")
                }
            };

            model.Subtotal = model.CartItems.Sum(i => i.Subtotal);
            model.Shipping = model.Subtotal >= 2500 ? 0 : 100; // Free shipping over 2500 EGP
            model.Tax = model.Subtotal * 0.14m; // 14% VAT (Egypt)
            model.Total = model.Subtotal + model.Shipping + model.Tax;

            return View(model);
        }

        /// <summary>
        /// Places the order from checkout form
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var userId = GetCurrentUserId();

            // Validate required fields
            if (string.IsNullOrEmpty(model.Street) || string.IsNullOrEmpty(model.City) ||
                string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Phone))
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction("Checkout");
            }

            // Validate credit card if payment method is Credit Card
            if (model.PaymentMethod == "Credit Card")
            {
                if (string.IsNullOrEmpty(model.CardNumber) || string.IsNullOrEmpty(model.CardholderName) ||
                    string.IsNullOrEmpty(model.ExpiryDate) || string.IsNullOrEmpty(model.CVV))
                {
                    TempData["ErrorMessage"] = "Please complete all credit card fields.";
                    return RedirectToAction("Checkout");
                }
            }

            var (success, message, order) = await _orderService.CreateOrderAsync(
                userId, model.Street, model.City, model.Email, model.Phone, model.PaymentMethod,
                model.CardNumber, model.CardholderName);

            if (!success || order == null)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Checkout");
            }

            return RedirectToAction("Success", new { id = order.OrderId });
        }

        /// <summary>
        /// Order success page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orderService.GetWithDetailsAsync(id);

            if (order == null || order.UserId != GetCurrentUserId())
            {
                return NotFound();
            }

            var model = new OrderSuccessViewModel
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Street = order.Street,
                City = order.City,
                Items = order.OrderDetails.Select(od => new OrderDetailViewModel
                {
                    ProductId = od.ProductId,
                    ProductName = od.ProductName ?? od.Product?.ProductName ?? "Unknown",
                    ProductImageUrl = od.ProductImageUrl ?? od.Product?.ImageUrl,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })
            };

            return View(model);
        }

        /// <summary>
        /// Order history list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> History(int page = 1)
        {
            const int pageSize = 10;
            var userId = GetCurrentUserId();

            var (orders, totalCount, totalPages) = await _orderService.GetPagedAsync(page, pageSize, userId);

            var model = new OrderHistoryViewModel
            {
                Orders = orders.Select(o => new OrderHistoryItemViewModel
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    Total = o.TotalAmount,
                    Status = o.Status,
                    TotalItems = o.ItemCount,
                    StatusBadgeClass = o.StatusBadgeClass
                }),
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount
            };

            return View(model);
        }

        /// <summary>
        /// Order details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetWithDetailsAsync(id);

            if (order == null || order.UserId != GetCurrentUserId())
            {
                return NotFound();
            }

            var shippingAddress = $"{order.Street}, {order.City}";

            var model = new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Total = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = shippingAddress,
                EmailAddress = order.EmailAddress,
                PhoneNumber = order.PhoneNumber,
                PaymentMethod = order.PaymentMethod,
                CardLast4Digits = order.CardLast4Digits,
                CardholderName = order.CardholderName,
                CardType = order.CardType,
                ShippedDate = order.ShippedDate,
                DeliveredDate = order.DeliveredDate,
                StatusBadgeClass = order.StatusBadgeClass,
                Items = order.OrderDetails.Select(od => new OrderDetailItemViewModel
                {
                    ProductId = od.ProductId,
                    ProductName = od.ProductName ?? od.Product?.ProductName ?? "Unknown",
                    ImageUrl = od.ProductImageUrl ?? od.Product?.ImageUrl,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })
            };

            return View(model);
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
