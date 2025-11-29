using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Arcade.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description", "DisplayOrder", "IconClass", "IsActive" },
                values: new object[,]
                {
                    { 1, "Laptop", "Gaming & Professional Laptops", 1, "fas fa-laptop", true },
                    { 2, "Monitor", "Gaming & Professional Monitors", 2, "fas fa-desktop", true },
                    { 3, "RAM", "DDR4 & DDR5 Memory Modules", 3, "fas fa-memory", true },
                    { 4, "SSD", "NVMe & SATA Solid State Drives", 4, "fas fa-hdd", true },
                    { 5, "Mouse", "Gaming & Productivity Mice", 5, "fas fa-mouse", true },
                    { 6, "Keyboard", "Mechanical & Membrane Keyboards", 6, "fas fa-keyboard", true },
                    { 7, "CPU", "Desktop Processors", 7, "fas fa-microchip", true },
                    { 8, "GPU", "Graphics Cards", 8, "fas fa-tv", true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FailedLoginAttempts", "FullName", "IsActive", "LastLoginAt", "LockoutEnd", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@arcade.com", 0, "Admin User", true, null, null, "$2a$11$rNzCv3xZCdQ9E4tH5mN8O.6JvK1lM2nP3qR4sT5uV6wX7yZ8aB9cD", "Admin" },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@arcade.com", 0, "John Doe", true, null, null, "$2a$11$aB1cD2eF3gH4iJ5kL6mN7O.8pQ9rS0tU1vW2xY3zA4bC5dE6fG7hI", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Brand", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Price", "ProductName", "SKU", "ShortDescription", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "ASUS", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASUS ROG Strix G15 Gaming Laptop with AMD Ryzen 9 7945HX, NVIDIA RTX 4070 8GB, 16GB DDR5 RAM, 1TB NVMe SSD, 15.6\" QHD 165Hz Display. Perfect for competitive gaming and content creation.", "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=600&h=400&fit=crop", true, true, 69999.00m, "ASUS ROG Strix G15", "ASUS-ROG-G15", "Ryzen 9, RTX 4070, 16GB RAM", 15, null },
                    { 2, "MSI", 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSI Katana 15 Gaming Laptop with Intel Core i7-13620H, NVIDIA RTX 4060 8GB, 16GB DDR5 RAM, 512GB NVMe SSD, 15.6\" FHD 144Hz Display. Excellent value for gaming enthusiasts.", "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?w=600&h=400&fit=crop", true, true, 45999.00m, "MSI Katana 15", "MSI-KATANA-15", "Intel i7, RTX 4060, 16GB RAM", 20, null },
                    { 3, "Lenovo", 1, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lenovo Legion Pro 5 Gaming Laptop with AMD Ryzen 7 7745HX, NVIDIA RTX 4070 8GB, 32GB DDR5 RAM, 1TB NVMe SSD, 16\" WQXGA 165Hz Display. Premium build quality with excellent cooling.", "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=600&h=400&fit=crop", true, true, 74999.00m, "Lenovo Legion Pro 5", "LENOVO-LEGION-PRO5", "Ryzen 7, RTX 4070, 32GB RAM", 12, null },
                    { 4, "Dell", 1, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dell G15 5530 Gaming Laptop with Intel Core i5-13450HX, NVIDIA RTX 4050 6GB, 8GB DDR5 RAM, 512GB NVMe SSD, 15.6\" FHD 120Hz Display. Budget-friendly gaming laptop.", "https://images.unsplash.com/photo-1588872657578-7efd1f1555ed?w=600&h=400&fit=crop", true, false, 32999.00m, "Dell G15 5530", "DELL-G15-5530", "Intel i5, RTX 4050, 8GB RAM", 25, null },
                    { 5, "Samsung", 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung Odyssey G7 32-inch Curved Gaming Monitor with QHD 2560x1440 resolution, 240Hz refresh rate, 1ms response time, HDR600, G-Sync Compatible. 1000R curvature for immersive gaming.", "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=600&h=400&fit=crop", true, true, 21999.00m, "Samsung Odyssey G7 32\"", "SAMSUNG-G7-32", "QHD 240Hz Curved Gaming Monitor", 18, null },
                    { 6, "LG", 2, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "LG UltraGear 27GP850 27-inch Gaming Monitor with Nano IPS technology, QHD 2560x1440 resolution, 165Hz refresh rate, 1ms response time, HDR400, G-Sync Compatible. Vibrant colors and fast response.", "https://images.unsplash.com/photo-1616711220408-a5e88d4cf0ad?w=600&h=400&fit=crop", true, true, 14999.00m, "LG UltraGear 27GP850", "LG-27GP850", "QHD 165Hz Nano IPS", 22, null },
                    { 7, "ASUS", 2, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASUS TUF Gaming VG279QM 27-inch Monitor with Fast IPS panel, FHD 1920x1080 resolution, 280Hz overclocked refresh rate, 1ms response time, G-Sync Compatible. ELMB Sync technology.", "https://images.unsplash.com/photo-1585792180666-f7347c490ee2?w=600&h=400&fit=crop", true, false, 9499.00m, "ASUS TUF Gaming 27\"", "ASUS-TUF-VG279", "FHD 165Hz IPS Monitor", 30, null },
                    { 8, "Dell", 2, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dell S2722DGM 27-inch Curved Gaming Monitor with VA panel, QHD 2560x1440 resolution, 165Hz refresh rate, 2ms response time, FreeSync Premium. 1500R curvature for comfortable viewing.", "https://images.unsplash.com/photo-1551645120-d70bfe84c826?w=600&h=400&fit=crop", true, false, 11999.00m, "Dell S2722DGM 27\"", "DELL-S2722DGM", "QHD 165Hz Curved VA", 20, null },
                    { 9, "G.Skill", 3, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "G.Skill Trident Z5 RGB 32GB DDR5 Memory Kit (2x16GB) running at 6000MHz with CL36 latency. Intel XMP 3.0 ready with stunning RGB lighting. Premium aluminum heatspreader.", "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?w=600&h=400&fit=crop", true, true, 7499.00m, "G.Skill Trident Z5 RGB 32GB", "GSKILL-Z5-32GB", "DDR5 6000MHz CL36 (2x16GB)", 40, null },
                    { 10, "Corsair", 3, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Corsair Vengeance DDR5 32GB Memory Kit (2x16GB) running at 5600MHz with CL40 latency. Intel XMP 3.0 compatible with iCUE software support. Sleek aluminum heatspreader design.", "https://images.unsplash.com/photo-1562976540-1502c2145186?w=600&h=400&fit=crop", true, false, 5999.00m, "Corsair Vengeance DDR5 32GB", "CORSAIR-VENG-32GB", "DDR5 5600MHz CL40 (2x16GB)", 35, null },
                    { 11, "Kingston", 3, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kingston Fury Beast 16GB DDR4 Memory Kit (2x8GB) running at 3200MHz with CL16 latency. Intel XMP ready, Plug N Play automatic overclocking. Low-profile heatspreader.", "https://images.unsplash.com/photo-1541140134513-85a161dc4a00?w=600&h=400&fit=crop", true, false, 2499.00m, "Kingston Fury Beast 16GB", "KINGSTON-FURY-16GB", "DDR4 3200MHz CL16 (2x8GB)", 50, null },
                    { 12, "TeamGroup", 3, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "TeamGroup T-Force Delta RGB 32GB DDR4 Memory Kit (2x16GB) running at 3600MHz with CL18 latency. Full-cover RGB lighting with 120° wide-angle design. Supports all major RGB software.", "https://images.unsplash.com/photo-1600861194942-f883de0dfe96?w=600&h=400&fit=crop", true, false, 3999.00m, "TeamGroup T-Force Delta RGB 32GB", "TEAM-DELTA-32GB", "DDR4 3600MHz CL18 (2x16GB)", 30, null },
                    { 13, "Samsung", 4, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung 990 Pro 2TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 7450MB/s sequential read and 6900MB/s write speeds. Samsung V-NAND technology with intelligent thermal control.", "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=600&h=400&fit=crop", true, true, 8999.00m, "Samsung 990 Pro 2TB", "SAMSUNG-990PRO-2TB", "PCIe 4.0 NVMe M.2 7450MB/s", 25, null },
                    { 14, "Western Digital", 4, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "WD Black SN850X 1TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 7300MB/s sequential read speeds. Game Mode 2.0 for predictive loading. Optional heatsink available.", "https://images.unsplash.com/photo-1531492746076-161ca9bcad58?w=600&h=400&fit=crop", true, true, 4999.00m, "WD Black SN850X 1TB", "WD-SN850X-1TB", "PCIe 4.0 NVMe M.2 7300MB/s", 30, null },
                    { 15, "Crucial", 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crucial P3 Plus 1TB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 5000MB/s sequential read speeds. Great value for everyday computing and gaming. Micron Advanced 3D NAND.", "https://images.unsplash.com/photo-1628557044797-f21d1d65f9e4?w=600&h=400&fit=crop", true, false, 2999.00m, "Crucial P3 Plus 1TB", "CRUCIAL-P3PLUS-1TB", "PCIe 4.0 NVMe M.2 5000MB/s", 45, null },
                    { 16, "Kingston", 4, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kingston NV2 500GB NVMe M.2 SSD with PCIe 4.0 interface, delivering up to 3500MB/s sequential read speeds. Compact M.2 2280 form factor. Low power consumption for cooler operation.", "https://images.unsplash.com/photo-1647503380147-dd2cf8fc3ac9?w=600&h=400&fit=crop", true, false, 1499.00m, "Kingston NV2 500GB", "KINGSTON-NV2-500GB", "PCIe 4.0 NVMe M.2 3500MB/s", 60, null },
                    { 17, "Logitech", 5, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logitech G Pro X Superlight 2 Wireless Gaming Mouse weighing only 60g. HERO 2 sensor with 32K DPI, LIGHTSPEED wireless, 95+ hour battery life. Esports-grade performance in an ultra-lightweight design.", "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=600&h=400&fit=crop", true, true, 6499.00m, "Logitech G Pro X Superlight 2", "LOGI-GPXSL2", "Wireless Gaming Mouse 60g", 25, null },
                    { 18, "Razer", 5, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razer DeathAdder V3 Pro Wireless Gaming Mouse with iconic ergonomic shape. Focus Pro 30K sensor, HyperSpeed wireless, 90 hour battery. Ultra-lightweight at only 63g for competitive gaming.", "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?w=600&h=400&fit=crop", true, true, 5999.00m, "Razer DeathAdder V3 Pro", "RAZER-DAV3PRO", "Wireless Ergonomic Mouse 63g", 20, null },
                    { 19, "SteelSeries", 5, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "SteelSeries Aerox 3 Wireless Gaming Mouse with honeycomb shell design at 66g. TrueMove Air sensor, 200+ hour battery life, water resistant IP54 rating. AquaBarrier protection included.", "https://images.unsplash.com/photo-1613141411244-0e4ac259d217?w=600&h=400&fit=crop", true, false, 3499.00m, "SteelSeries Aerox 3 Wireless", "SS-AEROX3-WL", "Ultra-Lightweight Gaming Mouse 66g", 35, null },
                    { 20, "HyperX", 5, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "HyperX Pulsefire Haste 2 Ultra-Lightweight Wired Gaming Mouse at just 53g. 26K sensor, 8000Hz polling rate, 100 hour battery (wireless version). Flexible paracord cable included.", "https://images.unsplash.com/photo-1623126908029-58cb08a2b272?w=600&h=400&fit=crop", true, false, 2199.00m, "HyperX Pulsefire Haste 2", "HX-HASTE2", "Wired Gaming Mouse 53g", 40, null },
                    { 21, "Corsair", 6, new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Corsair K100 RGB Optical-Mechanical Gaming Keyboard with OPX switches. iCUE control wheel, PBT double-shot keycaps, 44-zone LightEdge RGB, magnetic wrist rest. Ultimate flagship keyboard.", "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=600&h=400&fit=crop", true, true, 8999.00m, "Corsair K100 RGB", "CORSAIR-K100", "Optical-Mechanical Full-Size", 15, null },
                    { 22, "Razer", 6, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razer Huntsman V3 Pro TKL Gaming Keyboard with analog optical switches. Adjustable actuation, magnetic wrist rest, PBT keycaps, multi-function digital dial. Per-key RGB with Razer Chroma.", "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=600&h=400&fit=crop", true, true, 7499.00m, "Razer Huntsman V3 Pro TKL", "RAZER-HUNTSV3-TKL", "Analog Optical TKL Keyboard", 18, null },
                    { 23, "Logitech", 6, new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logitech G915 TKL LIGHTSPEED Wireless Gaming Keyboard with low-profile GL switches. LIGHTSPEED wireless, Bluetooth, 40 hour battery, aircraft-grade aluminum. Premium slim design.", "https://images.unsplash.com/photo-1595225476474-87563907a212?w=600&h=400&fit=crop", true, false, 5999.00m, "Logitech G915 TKL Lightspeed", "LOGI-G915TKL", "Wireless Low-Profile TKL", 22, null },
                    { 24, "HyperX", 6, new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "HyperX Alloy Origins 65 Compact Mechanical Keyboard with HyperX Red linear switches. Aircraft-grade aluminum body, PBT keycaps, per-key RGB lighting. 65% layout with arrow keys retained.", "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=600&h=400&fit=crop", true, false, 2999.00m, "HyperX Alloy Origins 65", "HX-ORIGINS65", "Compact Mechanical 65%", 30, null },
                    { 25, "AMD", 7, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AMD Ryzen 9 7950X3D 16-Core 32-Thread Desktop Processor with 3D V-Cache technology. 5.7GHz max boost, 128MB L3 cache, AM5 socket. The ultimate gaming and productivity processor.", "https://images.unsplash.com/photo-1591799265444-d66432b91588?w=600&h=400&fit=crop", true, true, 27999.00m, "AMD Ryzen 9 7950X3D", "AMD-7950X3D", "16-Core 3D V-Cache Gaming CPU", 10, null },
                    { 26, "Intel", 7, new DateTime(2024, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intel Core i9-14900K 24-Core (8P+16E) Desktop Processor. 6.0GHz max turbo, 36MB cache, LGA1700 socket. Unlocked for overclocking with Intel Thermal Velocity Boost.", "https://images.unsplash.com/photo-1555617981-dac3880eac6e?w=600&h=400&fit=crop", true, true, 25999.00m, "Intel Core i9-14900K", "INTEL-I9-14900K", "24-Core Flagship Desktop CPU", 12, null },
                    { 27, "AMD", 7, new DateTime(2024, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "AMD Ryzen 7 7800X3D 8-Core 16-Thread Desktop Processor with 3D V-Cache. 5.0GHz max boost, 96MB L3 cache, AM5 socket. Best value gaming processor with incredible performance.", "https://images.unsplash.com/photo-1592664474496-8f2c35acd655?w=600&h=400&fit=crop", true, true, 17499.00m, "AMD Ryzen 7 7800X3D", "AMD-7800X3D", "8-Core 3D V-Cache Gaming CPU", 20, null },
                    { 28, "Intel", 7, new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intel Core i5-14600K 14-Core (6P+8E) Desktop Processor. 5.3GHz max turbo, 24MB cache, LGA1700 socket. Unlocked for overclocking, excellent gaming and multitasking performance.", "https://images.unsplash.com/photo-1518770660439-4636190af475?w=600&h=400&fit=crop", true, false, 12999.00m, "Intel Core i5-14600K", "INTEL-I5-14600K", "14-Core Mid-Range Desktop CPU", 25, null },
                    { 29, "NVIDIA", 8, new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "NVIDIA GeForce RTX 4090 Founders Edition with 24GB GDDR6X memory. Ada Lovelace architecture, DLSS 3, 2520MHz boost clock. The most powerful consumer graphics card for 4K gaming and AI workloads.", "https://images.unsplash.com/photo-1591488320449-011701bb6704?w=600&h=400&fit=crop", true, true, 89999.00m, "NVIDIA RTX 4090 Founders Edition", "NVIDIA-RTX4090-FE", "24GB GDDR6X Flagship GPU", 5, null },
                    { 30, "ASUS", 8, new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASUS ROG Strix GeForce RTX 4080 SUPER with 16GB GDDR6X memory. Axial-tech fans, 3.15 slot design, 2640MHz boost clock. Premium cooling and RGB lighting for enthusiast builds.", "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=600&h=400&fit=crop", true, true, 54999.00m, "ASUS ROG Strix RTX 4080 SUPER", "ASUS-RTX4080S-STRIX", "16GB GDDR6X Premium GPU", 8, null },
                    { 31, "MSI", 8, new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSI Gaming X Trio GeForce RTX 4070 Ti SUPER with 16GB GDDR6X memory. TRI FROZR 3 cooling, 2640MHz boost clock. Excellent 1440p and 4K gaming performance.", "https://images.unsplash.com/photo-1555618254-5e06a5e9d678?w=600&h=400&fit=crop", true, false, 42999.00m, "MSI Gaming X Trio RTX 4070 Ti SUPER", "MSI-RTX4070TIS-GXT", "16GB GDDR6X High-End GPU", 12, null },
                    { 32, "Gigabyte", 8, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gigabyte GeForce RTX 4060 Eagle OC with 8GB GDDR6 memory. WINDFORCE cooling, 2475MHz boost clock, ultra-durable components. Perfect for 1080p high-refresh gaming.", "https://images.unsplash.com/photo-1622219809260-ce065fc5277f?w=600&h=400&fit=crop", true, false, 14999.00m, "Gigabyte RTX 4060 Eagle OC", "GIGA-RTX4060-EAGLE", "8GB GDDR6 Mainstream GPU", 30, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
