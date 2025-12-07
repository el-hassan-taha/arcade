using System.ComponentModel.DataAnnotations;
using Arcade.Models;

namespace Arcade.ViewModels
{
    /// <summary>
    /// ViewModel for checkout step 1 - Review Cart
    /// </summary>
    public class CheckoutReviewViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = Enumerable.Empty<CartItemViewModel>();
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>
    /// ViewModel for checkout step 2 - Delivery Address
    /// </summary>
    public class CheckoutAddressViewModel
    {
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

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Order Notes (Optional)")]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// ViewModel for checkout step 3 - Order Confirmation
    /// </summary>
    public class CheckoutConfirmViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = Enumerable.Empty<CartItemViewModel>();
        public CheckoutAddressViewModel DeliveryAddress { get; set; } = new CheckoutAddressViewModel();
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>
    /// ViewModel for order success page
    /// </summary>
    public class OrderSuccessViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public IEnumerable<OrderDetailViewModel> Items { get; set; } = Enumerable.Empty<OrderDetailViewModel>();
    }

    /// <summary>
    /// ViewModel for order history list - container for orders
    /// </summary>
    public class OrderHistoryViewModel
    {
        public IEnumerable<OrderHistoryItemViewModel> Orders { get; set; } = Enumerable.Empty<OrderHistoryItemViewModel>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// ViewModel for individual order in history list
    /// </summary>
    public class OrderHistoryItemViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public string StatusBadgeClass { get; set; } = string.Empty;
        public IEnumerable<OrderItemSummaryViewModel> Items { get; set; } = Enumerable.Empty<OrderItemSummaryViewModel>();
    }

    /// <summary>
    /// ViewModel for order item summary in history
    /// </summary>
    public class OrderItemSummaryViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    /// <summary>
    /// ViewModel for order summary in list
    /// </summary>
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public string StatusBadgeClass { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel for order details page
    /// </summary>
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string StatusBadgeClass { get; set; } = string.Empty;
        public IEnumerable<OrderDetailItemViewModel> Items { get; set; } = Enumerable.Empty<OrderDetailItemViewModel>();
    }

    /// <summary>
    /// ViewModel for order line item in detail view
    /// </summary>
    public class OrderDetailItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
        public decimal Total => UnitPrice * Quantity;
    }

    /// <summary>
    /// ViewModel for order line item
    /// </summary>
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }

    /// <summary>
    /// ViewModel for checkout page
    /// </summary>
    public class CheckoutViewModel
    {
        public IEnumerable<CartItemViewModel> Items { get; set; } = Enumerable.Empty<CartItemViewModel>();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }

        // Customer info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // Shipping address
        public string? Street { get; set; }
        public string? City { get; set; }

        // Payment
        public string PaymentMethod { get; set; } = "Credit Card";
        
        // Credit Card Details (only used when PaymentMethod is "Credit Card")
        public string? CardNumber { get; set; }
        public string? CardholderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
    }

    /// <summary>
    /// ViewModel for order confirmation/success page
    /// </summary>
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalItems { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ShippingName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string EstimatedDelivery { get; set; } = string.Empty;
        public IEnumerable<OrderDetailViewModel> Items { get; set; } = Enumerable.Empty<OrderDetailViewModel>();
    }
}
