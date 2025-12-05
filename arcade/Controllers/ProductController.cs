using Microsoft.AspNetCore.Mvc;
using Arcade.Services;
using Arcade.ViewModels;

namespace Arcade.Controllers
{
    /// <summary>
    /// Controller for product catalog
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Displays product listing with filters and pagination
        /// </summary>
        public async Task<IActionResult> Index(
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStockOnly = null,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDesc = false,
            int page = 1)
        {
            const int pageSize = 12;

            var (products, totalCount, totalPages) = await _productService.GetPagedAsync(
                page, pageSize, categoryId, minPrice, maxPrice, inStockOnly, searchTerm, sortBy, sortDesc);

            var categories = await _productService.GetCategoriesAsync();

            var model = new ProductListViewModel
            {
                Products = products,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                TotalItems = totalCount,
                PageSize = pageSize,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                InStockOnly = inStockOnly ?? false,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortDescending = sortDesc
            };

            // Set category name for display
            if (categoryId.HasValue)
            {
                var category = categories.FirstOrDefault(c => c.CategoryId == categoryId);
                model.CategoryName = category?.CategoryName;
            }

            return View(model);
        }

        /// <summary>
        /// Displays product details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Get related products from same category
            var relatedProducts = (await _productService.GetByCategoryAsync(product.CategoryId))
                .Where(p => p.ProductId != id)
                .Take(4);

            var model = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts,
                SelectedQuantity = 1
            };

            return View(model);
        }

        /// <summary>
        /// Searches products
        /// </summary>
        public async Task<IActionResult> Search(string? q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { searchTerm = q });
        }

        /// <summary>
        /// AJAX endpoint for quick search suggestions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> QuickSearch(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }

            var products = await _productService.SearchAsync(term);
            var results = products.Take(5).Select(p => new
            {
                id = p.ProductId,
                name = p.ProductName,
                price = p.Price,
                imageUrl = p.ImageUrl,
                category = p.Category?.CategoryName
            });

            return Json(results);
        }

        /// <summary>
        /// Filter products by category
        /// </summary>
        public IActionResult Category(int id)
        {
            return RedirectToAction("Index", new { categoryId = id });
        }
    }
}
