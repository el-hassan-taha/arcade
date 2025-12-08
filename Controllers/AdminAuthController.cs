using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Arcade.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Arcade.Controllers
{
    /// <summary>
    /// Admin authentication controller - completely separate from customer auth
    /// </summary>
    [Route("Admin")]
    public class AdminAuthController : Controller
    {
        private readonly Arcade.Services.IAuthenticationService _authService;
        private readonly ILogger<AdminAuthController> _logger;
        private const string ADMIN_SCHEME = "AdminScheme";

        public AdminAuthController(
            Arcade.Services.IAuthenticationService authService,
            ILogger<AdminAuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Admin login page
        /// </summary>
        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            // If already authenticated as admin, redirect to dashboard
            var adminAuth = await HttpContext.AuthenticateAsync(ADMIN_SCHEME);
            if (adminAuth.Succeeded && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            // Sign out any customer session
            await HttpContext.SignOutAsync("CustomerScheme");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new AdminLoginViewModel());
        }

        /// <summary>
        /// Process admin login - ONLY accepts Admin role
        /// </summary>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Authenticate user using LoginAsync
                var (success, message, user, token) = await _authService.LoginAsync(model.Email, model.Password);

                if (!success || user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    _logger.LogWarning("Failed admin login attempt for email: {Email}", model.Email);
                    return View(model);
                }

                // CRITICAL: Only allow Admin role
                if (user.Role != "Admin")
                {
                    ModelState.AddModelError(string.Empty, "Access denied. Admin credentials required.");
                    _logger.LogWarning("Non-admin user attempted admin login: {Email}", model.Email);
                    return View(model);
                }

                // Create claims for admin
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("AuthScheme", "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, ADMIN_SCHEME);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    IssuedUtc = DateTimeOffset.UtcNow
                }

;

                // Sign in with Admin scheme
                await HttpContext.SignInAsync(ADMIN_SCHEME, claimsPrincipal, authProperties);

                _logger.LogInformation("Admin logged in: {Email}", user.Email);

                // Redirect to admin dashboard
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl.StartsWith("/Admin"))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during admin login for email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return View(model);
            }
        }

        /// <summary>
        /// Admin logout
        /// </summary>
        [HttpPost("Logout")]
        [Authorize(Policy = "AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(ADMIN_SCHEME);
            _logger.LogInformation("Admin logged out");
            return RedirectToAction("Login", "AdminAuth");
        }

        /// <summary>
        /// Access denied page for admin
        /// </summary>
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
