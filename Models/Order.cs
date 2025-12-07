using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arcade.Models
{
    /// <summary>
    /// Represents a customer order
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // "Pending", "Processing", "Shipped", "Completed", "Cancelled"

        [Required(ErrorMessage = "Street address is required")]
        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        [Display(Name = "Street Address")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Credit Card"; // "Credit Card", "PayPal", "Cash on Delivery", "InstaPay"

        // Credit Card Details (only stored when PaymentMethod is "Credit Card")
        [StringLength(4)]
        [Display(Name = "Card Last 4 Digits")]
        public string? CardLast4Digits { get; set; }

        [StringLength(100)]
        [Display(Name = "Cardholder Name")]
        public string? CardholderName { get; set; }

        [StringLength(20)]
        [Display(Name = "Card Type")]
        public string? CardType { get; set; } // "Visa", "Mastercard", etc.

        public DateTime? ShippedDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // Computed properties
        [NotMapped]
        public string OrderNumber => $"ORD-{OrderId:D6}";

        [NotMapped]
        public int ItemCount => OrderDetails?.Sum(od => od.Quantity) ?? 0;

        [NotMapped]
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "badge-warning",
            "Processing" => "badge-info",
            "Shipped" => "badge-primary",
            "Completed" => "badge-success",
            "Cancelled" => "badge-danger",
            _ => "badge-secondary"
        };
    }
}
