using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arcade.Models
{
    /// <summary>
    /// Represents a product in the store
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; } = 0;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        [StringLength(100)]
        public string? SKU { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Category? Category { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // Computed properties
        [NotMapped]
        public bool IsInStock => StockQuantity > 0;

        [NotMapped]
        public bool IsLowStock => StockQuantity > 0 && StockQuantity < 10;

        [NotMapped]
        public string StockStatus => StockQuantity == 0 ? "Out of Stock" :
                                     StockQuantity < 10 ? "Low Stock" : "In Stock";
    }
}
