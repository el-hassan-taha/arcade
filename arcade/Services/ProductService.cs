using Arcade.Data.Repositories;
using Arcade.Models;

namespace Arcade.Services
{
    /// <summary>
    /// Service interface for Product operations
    /// </summary>
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetPagedAsync(
            int page,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStockOnly = null,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10);
        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<(bool Success, string Message)> DeleteAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<int> GetTotalProductCountAsync();
    }

    /// <summary>
    /// Service implementation for Product operations
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _productRepository.GetWithCategoryAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.FindAsync(p => p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8)
        {
            return await _productRepository.GetFeaturedAsync(count);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Product>();

            return await _productRepository.SearchAsync(searchTerm);
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetPagedAsync(
            int page,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStockOnly = null,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false)
        {
            var (products, totalCount) = await _productRepository.GetPagedAsync(
                page, pageSize, categoryId, minPrice, maxPrice, inStockOnly, searchTerm, sortBy, sortDescending);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return (products, totalCount, totalPages);
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10)
        {
            return await _productRepository.GetLowStockAsync(threshold);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.IsActive = true;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.ProductId);
            if (existingProduct == null) return false;

            existingProduct.ProductName = product.ProductName;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Brand = product.Brand;
            existingProduct.SKU = product.SKU;
            existingProduct.IsFeatured = product.IsFeatured;
            existingProduct.IsActive = product.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return (false, "Product not found.");

            // Check if product has order history
            if (await _productRepository.HasOrderDetailsAsync(productId))
            {
                // Soft delete - mark as inactive instead
                product.IsActive = false;
                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();
                return (true, "Product has been deactivated (has order history).");
            }

            _productRepository.Remove(product);
            await _productRepository.SaveChangesAsync();
            return (true, "Product deleted successfully.");
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            product.StockQuantity = Math.Max(0, quantity);
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetWithProductCountsAsync();
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            return await _productRepository.CountAsync(p => p.IsActive);
        }
    }
}
