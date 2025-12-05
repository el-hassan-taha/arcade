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

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; } = "United States";

        [StringLength(500)]
        public string? Notes { get; set; }

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
