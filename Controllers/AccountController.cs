using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Arcade.Services;
using Arcade.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Arcade.Controllers
{
    /// <summary>
    /// Controller for user authentication and account management
    /// </summary>
    public class AccountController : Controller
    {
        private readonly Arcade.Services.IAuthenticationService _authService;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            Arcade.Services.IAuthenticationService authService,
            IOrderService orderService,
            ICartService cartService,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _orderService = orderService;
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Displays the login page
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Processes user login - CUSTOMER ONLY
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message, user, token) = await _authService.LoginAsync(model.Email, model.Password);

            if (!success || user == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            // CRITICAL: Block admin login on customer page
            if (user.Role == "Admin")
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                _logger.LogWarning("Admin user attempted customer login: {Email}", model.Email);
                return View(model);
            }

            // CRITICAL: Only allow Customer role
            if (user.Role != "Customer")
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // Create claims for customer
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim("AuthScheme", "Customer")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CustomerScheme");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                "CustomerScheme",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Store JWT token in session if needed
            HttpContext.Session.SetString("JwtToken", token ?? string.Empty);

            TempData["SuccessMessage"] = $"Welcome back, {user.FullName}!";

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays the registration page
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Processes user registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Combine FirstName and LastName into FullName
            var fullName = $"{model.FirstName} {model.LastName}".Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = model.FullName;
            }

            var (success, message, user) = await _authService.RegisterAsync(model.Email, model.Password, fullName, model.Phone, model.Address);

            if (!success || user == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            // Auto-login after registration with CustomerScheme
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim("AuthScheme", "Customer")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CustomerScheme");
            await HttpContext.SignInAsync(
                "CustomerScheme",
                new ClaimsPrincipal(claimsIdentity));

            TempData["SuccessMessage"] = "Welcome to Arcade! Your account has been created successfully.";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Logs out the user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomerScheme");
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays user profile
        /// </summary>
        [Authorize(Policy = "CustomerOnly", AuthenticationSchemes = "CustomerScheme")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = GetCurrentUserId();
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var orders = await _orderService.GetUserOrdersAsync(userId);
            var nameParts = user.FullName?.Split(' ', 2) ?? Array.Empty<string>();

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName ?? string.Empty,
                FirstName = nameParts.Length > 0 ? nameParts[0] : "",
                LastName = nameParts.Length > 1 ? nameParts[1] : "",
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                MemberSince = user.CreatedAt,
                OrderCount = orders.Count()
            };

            ViewBag.OrderCount = orders.Count();
            ViewBag.CartItems = await _cartService.GetCartItemCountAsync(userId);

            return View(model);
        }

        /// <summary>
        /// Updates user profile
        /// </summary>
        [Authorize(Policy = "CustomerOnly", AuthenticationSchemes = "CustomerScheme")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                var user = await _authService.GetUserByIdAsync(userId);
                model.Email = user?.Email ?? "";
                model.Role = user?.Role ?? "";
                model.MemberSince = user?.CreatedAt;
                model.OrderCount = (await _orderService.GetUserOrdersAsync(userId)).Count();
                ViewBag.OrderCount = model.OrderCount;
                ViewBag.CartItems = await _cartService.GetCartItemCountAsync(userId);
                return View(model);
            }

            var currentUserId = GetCurrentUserId();

            // Check if email is being changed and if it's already in use
            var currentUser = await _authService.GetUserByIdAsync(currentUserId);
            if (currentUser != null && currentUser.Email != model.Email.ToLower().Trim())
            {
                var emailExists = await _authService.EmailExistsAsync(model.Email, currentUserId);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "This email is already in use by another account.");
                    model.Role = currentUser.Role;
                    model.MemberSince = currentUser.CreatedAt;
                    model.OrderCount = (await _orderService.GetUserOrdersAsync(currentUserId)).Count();
                    ViewBag.OrderCount = model.OrderCount;
                    ViewBag.CartItems = await _cartService.GetCartItemCountAsync(currentUserId);
                    return View(model);
                }
            }

            // Combine first and last name for full name
            var fullName = $"{model.FirstName} {model.LastName}".Trim();

            _logger.LogInformation($"Updating profile for user {currentUserId}: Name={fullName}, Email={model.Email}, Phone={model.Phone}, Address={model.Address}");

            var result = await _authService.UpdateProfileAsync(currentUserId, fullName, model.Email, model.Phone, model.Address);

            if (result)
            {
                _logger.LogInformation($"Profile update successful for user {currentUserId}");

                // Update authentication claims with new name and email
                var updatedUser = await _authService.GetUserByIdAsync(currentUserId);
                if (updatedUser != null)
                {
                    _logger.LogInformation($"Retrieved updated user: Name={updatedUser.FullName}, Email={updatedUser.Email}");

                    // Sign out the current user (CustomerScheme)
                    await HttpContext.SignOutAsync("CustomerScheme");

                    // Create new claims with updated information
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, updatedUser.UserId.ToString()),
                        new Claim(ClaimTypes.Email, updatedUser.Email),
                        new Claim(ClaimTypes.Name, updatedUser.FullName),
                        new Claim(ClaimTypes.Role, "Customer"),
                        new Claim("AuthScheme", "Customer")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CustomerScheme");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    // Sign in with updated claims (CustomerScheme)
                    await HttpContext.SignInAsync(
                        "CustomerScheme",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation($"Authentication claims updated for user {currentUserId}");
                }

                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            else
            {
                _logger.LogWarning($"Profile update failed for user {currentUserId}");
                TempData["ErrorMessage"] = "Failed to update profile. Please try again.";
            }

            return RedirectToAction("Profile");
        }

        /// <summary>
        /// Displays change password form
        /// </summary>
        [Authorize(Policy = "CustomerOnly", AuthenticationSchemes = "CustomerScheme")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Processes password change
        /// </summary>
        [Authorize(Policy = "CustomerOnly", AuthenticationSchemes = "CustomerScheme")]
        [HttpPost]
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
