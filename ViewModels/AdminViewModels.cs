using Arcade.Models;

namespace Arcade.ViewModels
{
    /// <summary>
    /// ViewModel for Admin Dashboard
    /// </summary>
    public class AdminDashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public IEnumerable<AdminRecentOrderViewModel> RecentOrders { get; set; } = Enumerable.Empty<AdminRecentOrderViewModel>();
        public IEnumerable<Product> LowStockProducts { get; set; } = Enumerable.Empty<Product>();
    }

    /// <summary>
    /// ViewModel for recent orders in admin dashboard
    /// </summary>
    public class AdminRecentOrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel for Admin Product List
    /// </summary>
    public class AdminProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 20;

        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? StockStatus { get; set; }
        public bool SortDescending { get; set; }

        // Total stock counts from entire database
        public int TotalInStockCount { get; set; }
        public int TotalLowStockCount { get; set; }
        public int TotalOutOfStockCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    /// <summary>
    /// ViewModel for Admin Order List
    /// </summary>
    public class AdminOrderListViewModel
    {
        public IEnumerable<Order> Orders { get; set; } = Enumerable.Empty<Order>();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 20;

        public string? Status { get; set; }
        public string? SearchTerm { get; set; }
        public string? Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    /// <summary>
    /// ViewModel for Admin Order Details
    /// </summary>
    public class AdminOrderDetailsViewModel
    {
        public Order Order { get; set; } = new Order();
        public string[] AvailableStatuses { get; set; } = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
    }

    /// <summary>
    /// ViewModel for Inventory Management
    /// </summary>
    public class InventoryViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public int LowStockThreshold { get; set; } = 10;
        public int TotalProducts { get; set; }
        public int InStockProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
    }

    /// <summary>
    /// ViewModel for Admin Order View - for individual order in list
    /// </summary>
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int TotalItems { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel for Admin Orders page
    /// </summary>
    public class AdminOrdersPageViewModel
    {
        public IEnumerable<AdminOrderViewModel> Orders { get; set; } = Enumerable.Empty<AdminOrderViewModel>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 20;
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    /// <summary>
    /// ViewModel for Admin Inventory View
    /// </summary>
    public class AdminInventoryViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public int LowStockThreshold { get; set; } = 10;
        public int TotalProducts { get; set; }
        public int InStockCount { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public int? CategoryFilter { get; set; }
        public string? StockFilter { get; set; }
    }
}
