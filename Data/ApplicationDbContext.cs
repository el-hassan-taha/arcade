using Microsoft.EntityFrameworkCore;
using Arcade.Models;
using BCrypt.Net;

namespace Arcade.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories - Computer Hardware
            var categories = new[]
            {
                new Category { CategoryId = 1, CategoryName = "Laptop", Description = "Gaming & Professional Laptops", IconClass = "fas fa-laptop", DisplayOrder = 1, IsActive = true },
                new Category { CategoryId = 2, CategoryName = "Monitor", Description = "Gaming & Professional Monitors", IconClass = "fas fa-desktop", DisplayOrder = 2, IsActive = true },
                new Category { CategoryId = 3, CategoryName = "RAM", Description = "DDR4 & DDR5 Memory Modules", IconClass = "fas fa-memory", DisplayOrder = 3, IsActive = true },
                new Category { CategoryId = 4, CategoryName = "SSD", Description = "NVMe & SATA Solid State Drives", IconClass = "fas fa-hdd", DisplayOrder = 4, IsActive = true },
                new Category { CategoryId = 5, CategoryName = "Mouse", Description = "Gaming & Productivity Mice", IconClass = "fas fa-mouse", DisplayOrder = 5, IsActive = true },
                new Category { CategoryId = 6, CategoryName = "Keyboard", Description = "Mechanical & Membrane Keyboards", IconClass = "fas fa-keyboard", DisplayOrder = 6, IsActive = true },
                new Category { CategoryId = 7, CategoryName = "CPU", Description = "Desktop Processors", IconClass = "fas fa-microchip", DisplayOrder = 7, IsActive = true },
                new Category { CategoryId = 8, CategoryName = "GPU", Description = "Graphics Cards", IconClass = "fas fa-tv", DisplayOrder = 8, IsActive = true }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Users - using pre-generated BCrypt hashes
            // Admin@123! hash (work factor 11)
            var adminPasswordHash = "$2a$11$Uw2NSbcyTC8wom1YEchTPuV.eTyITOscaKNikN9E54zf1HUZfI/qC";
            // Customer@123! hash (work factor 11)
            var userPasswordHash = "$2a$11$0vTyhMfGAY56h1v.gGbzHu8EtAh4usIUivBDlZCsPMLA6dmyBoUG6";

            var users = new[]
            {
                new User
                {
                    UserId = 1,
                    Email = "admin@arcade.com",
                    PasswordHash = adminPasswordHash,
                    FullName = "Admin User",
                    Role = "Admin",
                    CreatedAt = new DateTime(2024, 1, 1),
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    Email = "john@arcade.com",
                    PasswordHash = userPasswordHash,
                    FullName = "John Doe",
                    Role = "Customer",
                    CreatedAt = new DateTime(2024, 1, 15),
                    IsActive = true
                }
            };
            modelBuilder.Entity<User>().HasData(users);

            // Seed Products with EGP Prices
            var products = new[]
            {
                // Laptops (CategoryId = 1)
                new Product
                {
                    ProductId = 1,
                    ProductName = "ASUS ROG Strix G15",
                    ShortDescription = "Ryzen 9, RTX 4070, 16GB RAM",
                    Description = "ASUS ROG Strix G15 Gaming Laptop with AMD Ryzen 9 7945HX, NVIDIA RTX 4070 8GB, 16GB DDR5 RAM, 1TB NVMe SSD, 15.6\" QHD 165Hz Display. Perfect for competitive gaming and content creation.",
                    Price = 69999.00m,
                    Brand = "ASUS",
                    CategoryId = 1,
                    SKU = "ASUS-ROG-G15",
                    StockQuantity = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "MSI Katana 15",
                    ShortDescription = "Intel i7, RTX 4060, 16GB RAM",
                    Description = "MSI Katana 15 Gaming Laptop with Intel Core i7-13620H, NVIDIA RTX 4060 8GB, 16GB DDR5 RAM, 512GB NVMe SSD, 15.6\" FHD 144Hz Display. Excellent value for gaming enthusiasts.",
                    Price = 45999.00m,
                    Brand = "MSI",
                    CategoryId = 1,
                    SKU = "MSI-KATANA-15",
                    StockQuantity = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 2)
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Lenovo Legion Pro 5",
                    ShortDescription = "Ryzen 7, RTX 4070, 32GB RAM",
                    Description = "Lenovo Legion Pro 5 Gaming Laptop with AMD Ryzen 7 7745HX, NVIDIA RTX 4070 8GB, 32GB DDR5 RAM, 1TB NVMe SSD, 16\" WQXGA 165Hz Display. Premium build quality with excellent cooling.",
                    Price = 74999.00m,
                    Brand = "Lenovo",
                    CategoryId = 1,
                    SKU = "LENOVO-LEGION-PRO5",
                    StockQuantity = 12,
                    ImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 3)
                },
                new Product
                {
                    ProductId = 4,
                    ProductName = "Dell G15 5530",
                    ShortDescription = "Intel i5, RTX 4050, 8GB RAM",
                    Description = "Dell G15 5530 Gaming Laptop with Intel Core i5-13450HX, NVIDIA RTX 4050 6GB, 8GB DDR5 RAM, 512GB NVMe SSD, 15.6\" FHD 120Hz Display. Budget-friendly gaming laptop.",
                    Price = 32999.00m,
                    Brand = "Dell",
                    CategoryId = 1,
                    SKU = "DELL-G15-5530",
                    StockQuantity = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1588872657578-7efd1f1555ed?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 4)
                },

                // Monitors (CategoryId = 2)
                new Product
                {
                    ProductId = 5,
                    ProductName = "Samsung Odyssey G7 32\"",
                    ShortDescription = "QHD 240Hz Curved Gaming Monitor",
                    Description = "Samsung Odyssey G7 32-inch Curved Gaming Monitor with QHD 2560x1440 resolution, 240Hz refresh rate, 1ms response time, HDR600, G-Sync Compatible. 1000R curvature for immersive gaming.",
                    Price = 21999.00m,
                    Brand = "Samsung",
                    CategoryId = 2,
                    SKU = "SAMSUNG-G7-32",
                    StockQuantity = 18,
                    ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 5)
                },
                new Product
                {
                    ProductId = 6,
                    ProductName = "LG UltraGear 27GP850",
                    ShortDescription = "QHD 165Hz Nano IPS",
                    Description = "LG UltraGear 27GP850 27-inch Gaming Monitor with Nano IPS technology, QHD 2560x1440 resolution, 165Hz refresh rate, 1ms response time, HDR400, G-Sync Compatible. Vibrant colors and fast response.",
                    Price = 14999.00m,
                    Brand = "LG",
                    CategoryId = 2,
                    SKU = "LG-27GP850",
                    StockQuantity = 22,
                    ImageUrl = "https://images.unsplash.com/photo-1616711220408-a5e88d4cf0ad?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 6)
                },
                new Product
                {
                    ProductId = 7,
                    ProductName = "ASUS TUF Gaming 27\"",
                    ShortDescription = "FHD 165Hz IPS Monitor",
                    Description = "ASUS TUF Gaming VG279QM 27-inch Monitor with Fast IPS panel, FHD 1920x1080 resolution, 280Hz overclocked refresh rate, 1ms response time, G-Sync Compatible. ELMB Sync technology.",
                    Price = 9499.00m,
                    Brand = "ASUS",
                    CategoryId = 2,
                    SKU = "ASUS-TUF-VG279",
                    StockQuantity = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1585792180666-f7347c490ee2?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 7)
                },
                new Product
                {
                    ProductId = 8,
                    ProductName = "Dell S2722DGM 27\"",
                    ShortDescription = "QHD 165Hz Curved VA",
                    Description = "Dell S2722DGM 27-inch Curved Gaming Monitor with VA panel, QHD 2560x1440 resolution, 165Hz refresh rate, 2ms response time, FreeSync Premium. 1500R curvature for comfortable viewing.",
                    Price = 11999.00m,
                    Brand = "Dell",
                    CategoryId = 2,
                    SKU = "DELL-S2722DGM",
                    StockQuantity = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1551645120-d70bfe84c826?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 8)
                },

                // RAM (CategoryId = 3)
                new Product
                {
                    ProductId = 9,
                    ProductName = "G.Skill Trident Z5 RGB 32GB",
                    ShortDescription = "DDR5 6000MHz CL36 (2x16GB)",
                    Description = "G.Skill Trident Z5 RGB 32GB DDR5 Memory Kit (2x16GB) running at 6000MHz with CL36 latency. Intel XMP 3.0 ready with stunning RGB lighting. Premium aluminum heatspreader.",
                    Price = 7499.00m,
                    Brand = "G.Skill",
                    CategoryId = 3,
                    SKU = "GSKILL-Z5-32GB",
                    StockQuantity = 40,
                    ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 9)
                },
                new Product
                {
                    ProductId = 10,
                    ProductName = "Corsair Vengeance DDR5 32GB",
                    ShortDescription = "DDR5 5600MHz CL40 (2x16GB)",
                    Description = "Corsair Vengeance DDR5 32GB Memory Kit (2x16GB) running at 5600MHz with CL40 latency. Intel XMP 3.0 compatible with iCUE software support. Sleek aluminum heatspreader design.",
                    Price = 5999.00m,
                    Brand = "Corsair",
                    CategoryId = 3,
                    SKU = "CORSAIR-VENG-32GB",
                    StockQuantity = 35,
                    ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 10)
                },
                new Product
                {
                    ProductId = 11,
                    ProductName = "Kingston Fury Beast 16GB",
                    ShortDescription = "DDR4 3200MHz CL16 (2x8GB)",
                    Description = "Kingston Fury Beast 16GB DDR4 Memory Kit (2x8GB) running at 3200MHz with CL16 latency. Intel XMP ready, Plug N Play automatic overclocking. Low-profile heatspreader.",
                    Price = 2499.00m,
                    Brand = "Kingston",
                    CategoryId = 3,
                    SKU = "KINGSTON-FURY-16GB",
                    StockQuantity = 50,
                    ImageUrl = "https://images.unsplash.com/photo-1541140134513-85a161dc4a00?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 11)
                },
                new Product
                {
                    ProductId = 12,
                    ProductName = "TeamGroup T-Force Delta RGB 32GB",
                    ShortDescription = "DDR4 3600MHz CL18 (2x16GB)",
                    Description = "TeamGroup T-Force Delta RGB 32GB DDR4 Memory Kit (2x16GB) running at 3600MHz with CL18 latency. Full-cover RGB lighting with 120° wide-angle design. Supports all major RGB software.",
                    Price = 3999.00m,
                    Brand = "TeamGroup",
                    CategoryId = 3,
                    SKU = "TEAM-DELTA-32GB",
                    StockQuantity = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1600861194942-f883de0dfe96?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 12)
                },

                // SSD (CategoryId = 4)
                new Product
                {
                    ProductId = 13,
                    ProductName = "Samsung 990 Pro 2TB",
                    ShortDescription = "PCIe 4.0 NVMe M.2 7450MB/s",
                    Description = "Samsung 990 Pro 2TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 7450MB/s sequential read and 6900MB/s write speeds. Samsung V-NAND technology with intelligent thermal control.",
                    Price = 8999.00m,
                    Brand = "Samsung",
                    CategoryId = 4,
                    SKU = "SAMSUNG-990PRO-2TB",
                    StockQuantity = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 13)
                },
                new Product
                {
                    ProductId = 14,
                    ProductName = "WD Black SN850X 1TB",
                    ShortDescription = "PCIe 4.0 NVMe M.2 7300MB/s",
                    Description = "WD Black SN850X 1TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 7300MB/s sequential read speeds. Game Mode 2.0 for predictive loading. Optional heatsink available.",
                    Price = 4999.00m,
                    Brand = "Western Digital",
                    CategoryId = 4,
                    SKU = "WD-SN850X-1TB",
                    StockQuantity = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1531492746076-161ca9bcad58?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 14)
                },
                new Product
                {
                    ProductId = 15,
                    ProductName = "Crucial P3 Plus 1TB",
                    ShortDescription = "PCIe 4.0 NVMe M.2 5000MB/s",
                    Description = "Crucial P3 Plus 1TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 5000MB/s sequential read speeds. Great value for everyday computing and gaming. Micron Advanced 3D NAND.",
                    Price = 2999.00m,
                    Brand = "Crucial",
                    CategoryId = 4,
                    SKU = "CRUCIAL-P3PLUS-1TB",
                    StockQuantity = 45,
                    ImageUrl = "https://images.unsplash.com/photo-1628557044797-f21d1d65f9e4?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 15)
                },
                new Product
                {
                    ProductId = 16,
                    ProductName = "Kingston NV2 500GB",
                    ShortDescription = "PCIe 4.0 NVMe M.2 3500MB/s",
                    Description = "Kingston NV2 500GB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 3500MB/s sequential read speeds. Compact M.2 2280 form factor. Low power consumption for cooler operation.",
                    Price = 1499.00m,
                    Brand = "Kingston",
                    CategoryId = 4,
                    SKU = "KINGSTON-NV2-500GB",
                    StockQuantity = 60,
                    ImageUrl = "https://images.unsplash.com/photo-1647503380147-dd2cf8fc3ac9?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 16)
                },

                // Mouse (CategoryId = 5)
                new Product
                {
                    ProductId = 17,
                    ProductName = "Logitech G Pro X Superlight 2",
                    ShortDescription = "Wireless Gaming Mouse 60g",
                    Description = "Logitech G Pro X Superlight 2 Wireless Gaming Mouse weighing only 60g. HERO 2 sensor with 32K DPI, LIGHTSPEED wireless, 95+ hour battery life. Esports-grade performance in an ultra-lightweight design.",
                    Price = 6499.00m,
                    Brand = "Logitech",
                    CategoryId = 5,
                    SKU = "LOGI-GPXSL2",
                    StockQuantity = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 17)
                },
                new Product
                {
                    ProductId = 18,
                    ProductName = "Razer DeathAdder V3 Pro",
                    ShortDescription = "Wireless Ergonomic Mouse 63g",
                    Description = "Razer DeathAdder V3 Pro Wireless Gaming Mouse with iconic ergonomic shape. Focus Pro 30K sensor, HyperSpeed wireless, 90 hour battery. Ultra-lightweight at only 63g for competitive gaming.",
                    Price = 5999.00m,
                    Brand = "Razer",
                    CategoryId = 5,
                    SKU = "RAZER-DAV3PRO",
                    StockQuantity = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 18)
                },
                new Product
                {
                    ProductId = 19,
                    ProductName = "SteelSeries Aerox 3 Wireless",
                    ShortDescription = "Ultra-Lightweight Gaming Mouse 66g",
                    Description = "SteelSeries Aerox 3 Wireless Gaming Mouse with honeycomb shell design at 66g. TrueMove Air sensor, 200+ hour battery life, water resistant IP54 rating. AquaBarrier protection included.",
                    Price = 3499.00m,
                    Brand = "SteelSeries",
                    CategoryId = 5,
                    SKU = "SS-AEROX3-WL",
                    StockQuantity = 35,
                    ImageUrl = "https://images.unsplash.com/photo-1613141411244-0e4ac259d217?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 19)
                },
                new Product
                {
                    ProductId = 20,
                    ProductName = "HyperX Pulsefire Haste 2",
                    ShortDescription = "Wired Gaming Mouse 53g",
                    Description = "HyperX Pulsefire Haste 2 Ultra-Lightweight Wired Gaming Mouse at just 53g. 26K sensor, 8000Hz polling rate, 100 hour battery (wireless version). Flexible paracord cable included.",
                    Price = 2199.00m,
                    Brand = "HyperX",
                    CategoryId = 5,
                    SKU = "HX-HASTE2",
                    StockQuantity = 40,
                    ImageUrl = "https://images.unsplash.com/photo-1623126908029-58cb08a2b272?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 20)
                },

                // Keyboard (CategoryId = 6)
                new Product
                {
                    ProductId = 21,
                    ProductName = "Corsair K100 RGB",
                    ShortDescription = "Optical-Mechanical Full-Size",
                    Description = "Corsair K100 RGB Optical-Mechanical Gaming Keyboard with OPX switches. iCUE control wheel, PBT double-shot keycaps, 44-zone LightEdge RGB, magnetic wrist rest. Ultimate flagship keyboard.",
                    Price = 8999.00m,
                    Brand = "Corsair",
                    CategoryId = 6,
                    SKU = "CORSAIR-K100",
                    StockQuantity = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 21)
                },
                new Product
                {
                    ProductId = 22,
                    ProductName = "Razer Huntsman V3 Pro TKL",
                    ShortDescription = "Analog Optical TKL Keyboard",
                    Description = "Razer Huntsman V3 Pro TKL Gaming Keyboard with analog optical switches. Adjustable actuation, magnetic wrist rest, PBT keycaps, multi-function digital dial. Per-key RGB with Razer Chroma.",
                    Price = 7499.00m,
                    Brand = "Razer",
                    CategoryId = 6,
                    SKU = "RAZER-HUNTSV3-TKL",
                    StockQuantity = 18,
                    ImageUrl = "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 22)
                },
                new Product
                {
                    ProductId = 23,
                    ProductName = "Logitech G915 TKL Lightspeed",
                    ShortDescription = "Wireless Low-Profile TKL",
                    Description = "Logitech G915 TKL LIGHTSPEED Wireless Gaming Keyboard with low-profile GL switches. LIGHTSPEED wireless, Bluetooth, 40 hour battery, aircraft-grade aluminum. Premium slim design.",
                    Price = 5999.00m,
                    Brand = "Logitech",
                    CategoryId = 6,
                    SKU = "LOGI-G915TKL",
                    StockQuantity = 22,
                    ImageUrl = "https://images.unsplash.com/photo-1595225476474-87563907a212?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 23)
                },
                new Product
                {
                    ProductId = 24,
                    ProductName = "HyperX Alloy Origins 65",
                    ShortDescription = "Compact Mechanical 65%",
                    Description = "HyperX Alloy Origins 65 Compact Mechanical Keyboard with HyperX Red linear switches. Aircraft-grade aluminum body, PBT keycaps, per-key RGB lighting. 65% layout with arrow keys retained.",
                    Price = 2999.00m,
                    Brand = "HyperX",
                    CategoryId = 6,
                    SKU = "HX-ORIGINS65",
                    StockQuantity = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 24)
                },

                // CPU (CategoryId = 7)
                new Product
                {
                    ProductId = 25,
                    ProductName = "AMD Ryzen 9 7950X3D",
                    ShortDescription = "16-Core 3D V-Cache Gaming CPU",
                    Description = "AMD Ryzen 9 7950X3D 16-Core 32-Thread Desktop Processor with 3D V-Cache technology. 5.7GHz max boost, 128MB L3 cache, AM5 socket. The ultimate gaming and productivity processor.",
                    Price = 27999.00m,
                    Brand = "AMD",
                    CategoryId = 7,
                    SKU = "AMD-7950X3D",
                    StockQuantity = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1591799265444-d66432b91588?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 25)
                },
                new Product
                {
                    ProductId = 26,
                    ProductName = "Intel Core i9-14900K",
                    ShortDescription = "24-Core Flagship Desktop CPU",
                    Description = "Intel Core i9-14900K 24-Core (8P+16E) Desktop Processor. 6.0GHz max turbo, 36MB cache, LGA1700 socket. Unlocked for overclocking with Intel Thermal Velocity Boost.",
                    Price = 25999.00m,
                    Brand = "Intel",
                    CategoryId = 7,
                    SKU = "INTEL-I9-14900K",
                    StockQuantity = 12,
                    ImageUrl = "https://images.unsplash.com/photo-1555617981-dac3880eac6e?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 26)
                },
                new Product
                {
                    ProductId = 27,
                    ProductName = "AMD Ryzen 7 7800X3D",
                    ShortDescription = "8-Core 3D V-Cache Gaming CPU",
                    Description = "AMD Ryzen 7 7800X3D 8-Core 16-Thread Desktop Processor with 3D V-Cache. 5.0GHz max boost, 96MB L3 cache, AM5 socket. Best value gaming processor with incredible performance.",
                    Price = 17499.00m,
                    Brand = "AMD",
                    CategoryId = 7,
                    SKU = "AMD-7800X3D",
                    StockQuantity = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1592664474496-8f2c35acd655?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 27)
                },
                new Product
                {
                    ProductId = 28,
                    ProductName = "Intel Core i5-14600K",
                    ShortDescription = "14-Core Mid-Range Desktop CPU",
                    Description = "Intel Core i5-14600K 14-Core (6P+8E) Desktop Processor. 5.3GHz max turbo, 24MB cache, LGA1700 socket. Unlocked for overclocking, excellent gaming and multitasking performance.",
                    Price = 12999.00m,
                    Brand = "Intel",
                    CategoryId = 7,
                    SKU = "INTEL-I5-14600K",
                    StockQuantity = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 28)
                },

                // GPU (CategoryId = 8)
                new Product
                {
                    ProductId = 29,
                    ProductName = "NVIDIA RTX 4090 Founders Edition",
                    ShortDescription = "24GB GDDR6X Flagship GPU",
                    Description = "NVIDIA GeForce RTX 4090 Founders Edition with 24GB GDDR6X memory. Ada Lovelace architecture, DLSS 3, 2520MHz boost clock. The most powerful consumer graphics card for 4K gaming and AI workloads.",
                    Price = 89999.00m,
                    Brand = "NVIDIA",
                    CategoryId = 8,
                    SKU = "NVIDIA-RTX4090-FE",
                    StockQuantity = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1591488320449-011701bb6704?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 29)
                },
                new Product
                {
                    ProductId = 30,
                    ProductName = "ASUS ROG Strix RTX 4080 SUPER",
                    ShortDescription = "16GB GDDR6X Premium GPU",
                    Description = "ASUS ROG Strix GeForce RTX 4080 SUPER with 16GB GDDR6X memory. Axial-tech fans, 3.15 slot design, 2640MHz boost clock. Premium cooling and RGB lighting for enthusiast builds.",
                    Price = 54999.00m,
                    Brand = "ASUS",
                    CategoryId = 8,
                    SKU = "ASUS-RTX4080S-STRIX",
                    StockQuantity = 8,
                    ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=600&h=400&fit=crop",
                    IsFeatured = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 30)
                },
                new Product
                {
                    ProductId = 31,
                    ProductName = "MSI Gaming X Trio RTX 4070 Ti SUPER",
                    ShortDescription = "16GB GDDR6X High-End GPU",
                    Description = "MSI Gaming X Trio GeForce RTX 4070 Ti SUPER with 16GB GDDR6X memory. TRI FROZR 3 cooling, 2640MHz boost clock. Excellent 1440p and 4K gaming performance.",
                    Price = 42999.00m,
                    Brand = "MSI",
                    CategoryId = 8,
                    SKU = "MSI-RTX4070TIS-GXT",
                    StockQuantity = 12,
                    ImageUrl = "https://images.unsplash.com/photo-1555618254-5e06a5e9d678?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 31)
                },
                new Product
                {
                    ProductId = 32,
                    ProductName = "Gigabyte RTX 4060 Eagle OC",
                    ShortDescription = "8GB GDDR6 Mainstream GPU",
                    Description = "Gigabyte GeForce RTX 4060 Eagle OC with 8GB GDDR6 memory. WINDFORCE cooling, 2475MHz boost clock, ultra-durable components. Perfect for 1080p high-refresh gaming.",
                    Price = 14999.00m,
                    Brand = "Gigabyte",
                    CategoryId = 8,
                    SKU = "GIGA-RTX4060-EAGLE",
                    StockQuantity = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1622219809260-ce065fc5277f?w=600&h=400&fit=crop",
                    IsFeatured = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 1)
                }
            };
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
