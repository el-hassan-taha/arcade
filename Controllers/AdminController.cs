using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Arcade.Services;
using Arcade.ViewModels;
using Arcade.Models;
using Arcade.Data.Repositories;
using Arcade.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Arcade.Controllers
{
    /// <summary>
    /// Admin controller for dashboard and management
    /// </summary>
    [Authorize(Policy = "AdminOnly", AuthenticationSchemes = "AdminScheme")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly Services.IAuthenticationService _authService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IProductService productService,
            IOrderService orderService,
            IUserRepository userRepository,
            ApplicationDbContext context,
            Services.IAuthenticationService authService,
            ILogger<AdminController> logger)
        {
            _productService = productService;
            _orderService = orderService;
            _userRepository = userRepository;
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Admin dashboard
        /// </summary>
        [HttpGet("")]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var allProducts = await _productService.GetAllAsync();
                var productsList = allProducts.ToList();
                var lowStockProducts = await _productService.GetLowStockAsync(10);
                var recentOrders = await _orderService.GetRecentOrdersAsync(5);

                var model = new AdminDashboardViewModel
                {
                    TotalProducts = productsList.Count,
                    TotalOrders = (await _orderService.GetPagedAsync(1, 1)).TotalCount,
                    TotalRevenue = await _orderService.GetTotalRevenueAsync(),
                    TotalCustomers = await _userRepository.GetCustomerCountAsync(),
                    TodayOrders = await _orderService.GetTodayOrderCountAsync(),
                    TodayRevenue = await _orderService.GetTodayRevenueAsync(),
                    PendingOrders = await _orderService.GetPendingOrderCountAsync(),
                    ProcessingOrders = await _orderService.GetOrderCountByStatusAsync("Processing"),
                    ShippedOrders = await _orderService.GetOrderCountByStatusAsync("Shipped"),
                    DeliveredOrders = await _orderService.GetOrderCountByStatusAsync("Delivered"),
                    LowStockItems = productsList.Count(p => p.StockQuantity > 0 && p.StockQuantity < 10),
                    OutOfStockItems = productsList.Count(p => p.StockQuantity == 0),
                    RecentOrders = recentOrders.Select(o => new AdminRecentOrderViewModel
                    {
                        OrderId = o.OrderId,
                        OrderNumber = o.OrderNumber,
                        CustomerName = o.User?.FullName ?? "Unknown",
                        OrderDate = o.OrderDate,
                        Total = o.TotalAmount,
                        Status = o.Status
                    }),
                    LowStockProducts = lowStockProducts.Take(5)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                TempData["ErrorMessage"] = "Failed to load dashboard data. Please try again.";
                return View(new AdminDashboardViewModel());
            }
        }

        #region Profile

        /// <summary>
        /// Admin profile (separate from customer profile)
        /// </summary>
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var names = (user.FullName ?? string.Empty).Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = names.Length > 0 ? names[0] : string.Empty;
            var lastName = names.Length > 1 ? names[1] : string.Empty;

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                FirstName = firstName,
                LastName = lastName,
                Role = user.Role,
                MemberSince = user.CreatedAt
            };

            return View(model);
        }

        /// <summary>
        /// Updates admin profile (name only)
        /// </summary>
        [HttpPost("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var fullName = $"{model.FirstName} {model.LastName}".Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullName = model.FullName;
            }

            // Check if email is being changed and if it's already taken
            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var existingUser = await _userRepository.FindAsync(u => u.Email == model.Email);
                if (existingUser.Any())
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View(model);
                }
                user.Email = model.Email;
            }

            user.FullName = fullName;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            // Update claims to reflect new name and email using AdminScheme
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AuthScheme", "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
            await HttpContext.SignInAsync(
                "AdminScheme",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }

        /// <summary>
        /// Admin change password (admin layout)
        /// </summary>
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Processes admin password change
        /// </summary>
        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetCurrentUserId();
            var (success, message) = await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        #endregion

        #region Product Management

        /// <summary>
        /// List all products
        /// </summary>
        [HttpGet("Products")]
        public async Task<IActionResult> Products(
            int? categoryId = null,
            string? searchTerm = null,
            string? brand = null,
            string? sku = null,
            string? stockStatus = null,
            string? sortBy = null,
            bool sortDesc = false,
            int page = 1)
        {
            const int pageSize = 20;

            // Get products based on search/category filter (without stock status filter first)
            var (products, totalCount, totalPages) = await _productService.GetPagedAsync(
                page, pageSize, categoryId, null, null, null, searchTerm, sortBy, sortDesc);

            // Get all products matching the current search/category filter for stats calculation
            var allFilteredProducts = await _productService.GetAllAsync();
            var filteredProductsList = allFilteredProducts.ToList();

            // Apply category filter for stats
            if (categoryId.HasValue)
            {
                filteredProductsList = filteredProductsList.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Apply search filter for stats
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProductsList = filteredProductsList.Where(p =>
                    p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (p.SKU != null && p.SKU.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // Apply brand filter for stats
            if (!string.IsNullOrEmpty(brand))
            {
                filteredProductsList = filteredProductsList.Where(p =>
                    p.Brand != null && p.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply SKU filter for stats
            if (!string.IsNullOrEmpty(sku))
            {
                filteredProductsList = filteredProductsList.Where(p =>
                    p.SKU != null && p.SKU.Contains(sku, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Calculate stock counts from filtered results
            int totalInStockCount = filteredProductsList.Count(p => p.StockQuantity >= 10);
            int totalLowStockCount = filteredProductsList.Count(p => p.StockQuantity > 0 && p.StockQuantity < 10);
            int totalOutOfStockCount = filteredProductsList.Count(p => p.StockQuantity == 0);

            // Apply brand filter for display
            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand != null && p.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase));
            }

            // Apply SKU filter for display
            if (!string.IsNullOrEmpty(sku))
            {
                products = products.Where(p => p.SKU != null && p.SKU.Contains(sku, StringComparison.OrdinalIgnoreCase));
            }

            // Apply stock status filter for display
            if (!string.IsNullOrEmpty(stockStatus))
            {
                products = stockStatus switch
                {
                    "instock" => products.Where(p => p.StockQuantity >= 10),
                    "lowstock" => products.Where(p => p.StockQuantity > 0 && p.StockQuantity < 10),
                    "outofstock" => products.Where(p => p.StockQuantity == 0),
                    _ => products
                };
            }

            var categories = await _productService.GetCategoriesAsync();

            var model = new AdminProductListViewModel
            {
                Products = products,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                TotalItems = filteredProductsList.Count,
                PageSize = pageSize,
                CategoryId = categoryId,
                SearchTerm = searchTerm,
                Brand = brand,
                SKU = sku,
                StockStatus = stockStatus,
                SortBy = sortBy,
                SortDescending = sortDesc,
                TotalInStockCount = totalInStockCount,
                TotalLowStockCount = totalLowStockCount,
                TotalOutOfStockCount = totalOutOfStockCount
            };

            return View(model);
        }

        /// <summary>
        /// Create product form
        /// </summary>
        [HttpGet("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _productService.GetCategoriesAsync();

            var model = new ProductFormViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }),
                IsActive = true
            };

            return View("ProductForm", model);
        }

        /// <summary>
        /// Create product
        /// </summary>
        [HttpPost("CreateProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _productService.GetCategoriesAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                });
                return View("ProductForm", model);
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                CategoryId = model.CategoryId,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Brand = model.Brand,
                SKU = model.SKU,
                IsFeatured = model.IsFeatured,
                IsActive = model.IsActive
            };

            await _productService.CreateAsync(product);

            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction("Products");
        }

        /// <summary>
        /// Edit product form
        /// </summary>
        [HttpGet("EditProduct/{id}")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = await _productService.GetCategoriesAsync();

            var model = new ProductFormViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand,
                SKU = product.SKU,
                IsFeatured = product.IsFeatured,
                IsActive = product.IsActive,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
            };

            return View("ProductForm", model);
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPost("EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _productService.GetCategoriesAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                });
                return View("ProductForm", model);
            }

            var product = new Product
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                CategoryId = model.CategoryId,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Brand = model.Brand,
                SKU = model.SKU,
                IsFeatured = model.IsFeatured,
                IsActive = model.IsActive
            };

            var success = await _productService.UpdateAsync(product);

            if (success)
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update product.";
            }

            return RedirectToAction("Products");
        }

        /// <summary>
        /// Delete product
        /// </summary>
        [HttpPost("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var (success, message) = await _productService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction("Products");
        }

        /// <summary>
        /// Quick update stock
        /// </summary>
        [HttpPost("UpdateStock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int productId, int quantity)
        {
            var success = await _productService.UpdateStockAsync(productId, quantity);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message = success ? "Stock updated" : "Failed to update stock" });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] =
                success ? "Stock updated successfully!" : "Failed to update stock.";

            // Redirect back to Inventory page if coming from there, otherwise Products
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/Inventory", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Inventory");
            }
            return RedirectToAction("Products");
        }

        #endregion

        #region Order Management

        /// <summary>
        /// List all orders
        /// </summary>
        [HttpGet("Orders")]
        public async Task<IActionResult> Orders(
            string? status = null,
            string? search = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int page = 1)
        {
            const int pageSize = 20;

            var (orders, totalCount, totalPages) = await _orderService.GetPagedAsync(page, pageSize, null, status, search);

            var model = new AdminOrderListViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                PageSize = pageSize,
                SearchTerm = search,
                Status = status,
                Search = search,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(model);
        }

        /// <summary>
        /// View order details
        /// </summary>
        [HttpGet("OrderDetails/{id}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetWithDetailsAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var model = new AdminOrderDetailsViewModel
            {
                Order = order
            };

            return View(model);
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPost("UpdateOrderStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var success = await _orderService.UpdateStatusAsync(id, status);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message = success ? "Status updated" : "Failed to update status" });
            }

            TempData[success ? "SuccessMessage" : "ErrorMessage"] =
                success ? "Order status updated!" : "Failed to update order status.";

            // Check if request came from OrderDetails page
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("OrderDetails"))
            {
                return RedirectToAction("OrderDetails", new { id });
            }

            return RedirectToAction("Orders");
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Inventory management view
        /// </summary>
        [HttpGet("Inventory")]
        public async Task<IActionResult> Inventory()
        {
            var allProducts = await _productService.GetAllAsync();
            var productsList = allProducts.ToList();

            var model = new AdminInventoryViewModel
            {
                Products = productsList.OrderBy(p => p.StockQuantity),
                TotalProducts = productsList.Count,
                InStockCount = productsList.Count(p => p.StockQuantity >= 10),
                LowStockCount = productsList.Count(p => p.StockQuantity > 0 && p.StockQuantity < 10),
                OutOfStockCount = productsList.Count(p => p.StockQuantity == 0)
            };

            return View(model);
        }

        #endregion

        #region Data Seeding

        /// <summary>
        /// Seed real products (development only)
        /// </summary>
        [HttpPost("SeedRealProducts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeedRealProducts()
        {
            try
            {
                await ProductSeeder.SeedRealProductsAsync(_context);
                TempData["SuccessMessage"] = "Real products seeded successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed products");
                TempData["ErrorMessage"] = $"Failed to seed products: {ex.Message}";
            }

            return RedirectToAction("Products");
        }

        #endregion

        #region Helpers

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
        }

        #endregion
    }
}
