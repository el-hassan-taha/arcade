using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arcade.Models
{
    /// <summary>
    /// Represents a line item in an order
    /// </summary>
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        // Snapshot of product details at time of order (for historical accuracy)
        [StringLength(200)]
        public string? ProductName { get; set; }

        [StringLength(500)]
        public string? ProductImageUrl { get; set; }

        // Navigation properties
        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }

        // Computed properties
        [NotMapped]
        public decimal Subtotal => UnitPrice * Quantity;
    }
}
