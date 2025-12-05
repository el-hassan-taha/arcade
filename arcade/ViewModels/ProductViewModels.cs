using System.ComponentModel.DataAnnotations;
using Arcade.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arcade.ViewModels
{
    /// <summary>
    /// ViewModel for product listing with pagination and filters
    /// </summary>
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 12;

        // Filters
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool InStockOnly { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    /// <summary>
    /// ViewModel for product details
    /// </summary>
    public class ProductDetailViewModel
    {
        public Product Product { get; set; } = new Product();
        public IEnumerable<Product> RelatedProducts { get; set; } = Enumerable.Empty<Product>();
        public int SelectedQuantity { get; set; } = 1;
    }

    /// <summary>
    /// ViewModel for creating/editing products (Admin)
    /// </summary>
    public class ProductFormViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        [StringLength(100)]
        public string? SKU { get; set; }

        [Display(Name = "Featured Product")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Dropdown options
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    }

    /// <summary>
    /// ViewModel for quick search results
    /// </summary>
    public class SearchResultViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public int ResultCount { get; set; }
    }
}
