using Microsoft.AspNetCore.Mvc;
using Arcade.Services;
using Arcade.ViewModels;

namespace Arcade.Controllers
{
    /// <summary>
    /// Controller for home and landing pages
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Displays the home page with featured products
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _productService.GetFeaturedAsync(8);
            var categories = await _productService.GetCategoriesAsync();

            ViewBag.FeaturedProducts = featuredProducts;
            ViewBag.Categories = categories;

            return View();
        }

        /// <summary>
        /// Displays the privacy policy page
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the about us page
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Displays the contact page
        /// </summary>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Displays the returns policy page
        /// </summary>
        public IActionResult Returns()
        {
            return View();
        }

        /// <summary>
        /// Displays the shipping information page
        /// </summary>
        public IActionResult Shipping()
        {
            return View();
        }

        /// <summary>
        /// Displays the FAQ page
        /// </summary>
        public IActionResult FAQ()
        {
            return View();
        }

        /// <summary>
        /// Displays access denied page
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
