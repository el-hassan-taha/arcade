using Arcade.Models;
using Microsoft.EntityFrameworkCore;

namespace Arcade.Data
{
    public static class ProductSeeder
    {
        public static async Task SeedRealProductsAsync(ApplicationDbContext context)
        {
            // Clear existing products
            var existingProducts = await context.Products.ToListAsync();
            context.Products.RemoveRange(existingProducts);
            await context.SaveChangesAsync();

            // Get categories
            var categories = await context.Categories.ToListAsync();
            var categoryDict = categories.ToDictionary(c => c.CategoryName, c => c.CategoryId);

            var products = new List<Product>();

            // === LAPTOPS ===
            if (categoryDict.ContainsKey("Laptop"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "HP Victus 15 Gaming Laptop",
                        Description = "لابتوب جيمنج HP Victus 15 بمعالج Intel Core i5-13420H وكارت شاشة RTX 3050 6GB وذاكرة 8GB RAM وقرص SSD 512GB وشاشة 15.6 بوصة FHD 144Hz",
                        Price = 33500.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=500",
                        IsFeatured = true,
                        Brand = "HP"
                    },
                    new Product
                    {
                        ProductName = "ASUS TUF Gaming F16",
                        Description = "لابتوب جيمنج ASUS TUF F16 بمعالج Intel Core 5 210H وكارت شاشة RTX 4050 6GB وذاكرة 8GB DDR5 RAM وقرص SSD 512GB وشاشة 16 بوصة FHD 144Hz",
                        Price = 41500.00m,
                        StockQuantity = 5,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500",
                        IsFeatured = true,
                        Brand = "ASUS"
                    },
                    new Product
                    {
                        ProductName = "Lenovo Legion 5 Pro",
                        Description = "لابتوب جيمنج Lenovo Legion 5 Pro بمعالج AMD Ryzen 7 وكارت شاشة RTX 3070 وذاكرة 16GB RAM وقرص SSD 1TB وشاشة 16 بوصة QHD 165Hz",
                        Price = 55000.00m,
                        StockQuantity = 4,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1593642632559-0c6d3fc62b89?w=500",
                        IsFeatured = true,
                        Brand = "Lenovo"
                    },
                    new Product
                    {
                        ProductName = "Dell G15 Gaming",
                        Description = "لابتوب جيمنج Dell G15 بمعالج Intel Core i7-12700H وكارت شاشة RTX 3060 وذاكرة 16GB RAM وقرص SSD 512GB وشاشة 15.6 بوصة FHD 165Hz",
                        Price = 44999.00m,
                        StockQuantity = 6,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1588872657578-7efd1f1555ed?w=500",
                        Brand = "Dell"
                    },
                    new Product
                    {
                        ProductName = "MSI Katana GF66",
                        Description = "لابتوب جيمنج MSI Katana GF66 بمعالج Intel Core i7 وكارت شاشة RTX 3070 وذاكرة 16GB RAM وقرص SSD 512GB وشاشة 15.6 بوصة FHD 144Hz",
                        Price = 49999.00m,
                        StockQuantity = 5,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1602080858428-57174f9431cf?w=500",
                        Brand = "MSI"
                    },
                    new Product
                    {
                        ProductName = "Acer Nitro 5",
                        Description = "لابتوب جيمنج Acer Nitro 5 بمعالج AMD Ryzen 5 6600H وكارت شاشة RTX 3050 Ti وذاكرة 8GB RAM وقرص SSD 512GB وشاشة 15.6 بوصة FHD 144Hz",
                        Price = 28999.00m,
                        StockQuantity = 10,
                        CategoryId = categoryDict["Laptop"],
                        ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=500",
                        Brand = "Acer"
                    }
                });
            }

            // === MONITORS ===
            if (categoryDict.ContainsKey("Monitor"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "MSI PRO MP251 24 inch",
                        Description = "شاشة MSI PRO MP251 مقاس 24 بوصة بدقة Full HD ومعدل تحديث 100Hz وتقنية Anti-Flicker وBlue Light Protection",
                        Price = 4500.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=500",
                        Brand = "MSI"
                    },
                    new Product
                    {
                        ProductName = "MSI G2412F Gaming Monitor",
                        Description = "شاشة جيمنج MSI G2412F مقاس 24 بوصة بدقة Full HD ومعدل تحديث 180Hz وتقنية Rapid IPS ووقت استجابة 1ms",
                        Price = 7000.00m,
                        StockQuantity = 10,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1585792180666-f7347c490ee2?w=500",
                        IsFeatured = true,
                        Brand = "MSI"
                    },
                    new Product
                    {
                        ProductName = "GIGABYTE GS25F2 Gaming Monitor",
                        Description = "شاشة جيمنج GIGABYTE GS25F2 مقاس 24.5 بوصة بدقة Full HD ومعدل تحديث 180Hz وتقنية SS IPS",
                        Price = 5800.00m,
                        StockQuantity = 12,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1616711906333-23cf8e1e10aa?w=500",
                        IsFeatured = true,
                        Brand = "GIGABYTE"
                    },
                    new Product
                    {
                        ProductName = "MSI PRO MP275Q 27 inch",
                        Description = "شاشة MSI PRO MP275Q مقاس 27 بوصة بدقة QHD 2560x1440 ومعدل تحديث 100Hz وتقنية Rapid IPS",
                        Price = 8500.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1593640408182-31c70c8268f5?w=500",
                        Brand = "MSI"
                    },
                    new Product
                    {
                        ProductName = "ViewSonic XG2409 Gaming",
                        Description = "شاشة جيمنج ViewSonic XG2409 مقاس 23.8 بوصة بدقة Full HD ومعدل تحديث 180Hz وتقنية IPS",
                        Price = 7300.00m,
                        StockQuantity = 7,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1551645120-d70bfe84c826?w=500",
                        Brand = "ViewSonic"
                    },
                    new Product
                    {
                        ProductName = "MSI 32C6 Curved Monitor",
                        Description = "شاشة منحنية MSI 32C6 مقاس 31.5 بوصة بدقة Full HD ومعدل تحديث 180Hz وتقنية VA Panel",
                        Price = 13000.00m,
                        StockQuantity = 5,
                        CategoryId = categoryDict["Monitor"],
                        ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=500",
                        Brand = "MSI"
                    }
                });
            }

            // === RAM ===
            if (categoryDict.ContainsKey("RAM"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "Kingston Fury Beast DDR5 16GB",
                        Description = "رامات Kingston Fury Beast بسعة 16GB من نوع DDR5 بسرعة 5200MHz مع إضاءة RGB",
                        Price = 3500.00m,
                        StockQuantity = 20,
                        CategoryId = categoryDict["RAM"],
                        ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?w=500",
                        Brand = "Kingston"
                    },
                    new Product
                    {
                        ProductName = "Corsair Vengeance DDR5 32GB Kit",
                        Description = "رامات Corsair Vengeance بسعة 32GB (2x16GB) من نوع DDR5 بسرعة 5600MHz",
                        Price = 6499.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["RAM"],
                        ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?w=500",
                        IsFeatured = true,
                        Brand = "Corsair"
                    },
                    new Product
                    {
                        ProductName = "G.Skill Trident Z5 RGB DDR5 32GB",
                        Description = "رامات G.Skill Trident Z5 RGB بسعة 32GB (2x16GB) من نوع DDR5 بسرعة 6000MHz وإضاءة RGB فاخرة",
                        Price = 8999.00m,
                        StockQuantity = 10,
                        CategoryId = categoryDict["RAM"],
                        ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=500",
                        IsFeatured = true,
                        Brand = "G.Skill"
                    },
                    new Product
                    {
                        ProductName = "TeamGroup T-Force Delta RGB DDR4 16GB",
                        Description = "رامات TeamGroup T-Force Delta RGB بسعة 16GB (2x8GB) من نوع DDR4 بسرعة 3200MHz وإضاءة RGB",
                        Price = 2499.00m,
                        StockQuantity = 25,
                        CategoryId = categoryDict["RAM"],
                        ImageUrl = "https://images.unsplash.com/photo-1555617117-08d2e0295b82?w=500",
                        Brand = "TeamGroup"
                    },
                    new Product
                    {
                        ProductName = "Crucial DDR4 8GB",
                        Description = "رامات Crucial بسعة 8GB من نوع DDR4 بسرعة 3200MHz - الخيار الاقتصادي الممتاز",
                        Price = 1200.00m,
                        StockQuantity = 30,
                        CategoryId = categoryDict["RAM"],
                        ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=500",
                        Brand = "Crucial"
                    }
                });
            }

            // === SSD ===
            if (categoryDict.ContainsKey("SSD"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "Samsung 990 Pro NVMe 1TB",
                        Description = "قرص تخزين Samsung 990 Pro NVMe SSD بسعة 1TB وسرعة قراءة 7450MB/s وكتابة 6900MB/s مع تقنية PCIe 4.0",
                        Price = 5500.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["SSD"],
                        ImageUrl = "https://images.unsplash.com/photo-1597138804456-e7dca7f59d54?w=500",
                        IsFeatured = true,
                        Brand = "Samsung"
                    },
                    new Product
                    {
                        ProductName = "WD Black SN850X 1TB",
                        Description = "قرص تخزين WD Black SN850X NVMe SSD بسعة 1TB وسرعة قراءة 7300MB/s مع تقنية PCIe 4.0",
                        Price = 5000.00m,
                        StockQuantity = 12,
                        CategoryId = categoryDict["SSD"],
                        ImageUrl = "https://images.unsplash.com/photo-1628557044797-f21a177c37ec?w=500",
                        Brand = "WD"
                    },
                    new Product
                    {
                        ProductName = "Kingston KC3000 512GB",
                        Description = "قرص تخزين Kingston KC3000 NVMe SSD بسعة 512GB وسرعة قراءة 7000MB/s مع تقنية PCIe 4.0",
                        Price = 2800.00m,
                        StockQuantity = 20,
                        CategoryId = categoryDict["SSD"],
                        ImageUrl = "https://images.unsplash.com/photo-1531492746076-161ca9bcad58?w=500",
                        Brand = "Kingston"
                    },
                    new Product
                    {
                        ProductName = "SanDisk Portable SSD 1TB",
                        Description = "قرص تخزين SanDisk Extreme Portable SSD خارجي بسعة 1TB وسرعة نقل حتى 1050MB/s",
                        Price = 4200.00m,
                        StockQuantity = 18,
                        CategoryId = categoryDict["SSD"],
                        ImageUrl = "https://images.unsplash.com/photo-1625225233840-695456021cde?w=500",
                        IsFeatured = true,
                        Brand = "SanDisk"
                    },
                    new Product
                    {
                        ProductName = "Crucial P5 Plus 500GB",
                        Description = "قرص تخزين Crucial P5 Plus NVMe SSD بسعة 500GB وسرعة قراءة 6600MB/s مع تقنية PCIe 4.0",
                        Price = 2200.00m,
                        StockQuantity = 22,
                        CategoryId = categoryDict["SSD"],
                        ImageUrl = "https://images.unsplash.com/photo-1541140532154-b024d705b90a?w=500",
                        Brand = "Crucial"
                    }
                });
            }

            // === MOUSE ===
            if (categoryDict.ContainsKey("Mouse"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "Logitech G102 Gaming Mouse",
                        Description = "ماوس جيمنج Logitech G102 Lightsync بإضاءة RGB ومستشعر بدقة 8000 DPI وتصميم مريح",
                        Price = 1000.00m,
                        StockQuantity = 25,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=500",
                        Brand = "Logitech"
                    },
                    new Product
                    {
                        ProductName = "Logitech G502 HERO Gaming",
                        Description = "ماوس جيمنج Logitech G502 HERO بمستشعر HERO بدقة 25600 DPI و11 زر قابل للبرمجة وإضاءة RGB",
                        Price = 2250.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?w=500",
                        IsFeatured = true,
                        Brand = "Logitech"
                    },
                    new Product
                    {
                        ProductName = "Redragon M601 RGB Gaming",
                        Description = "ماوس جيمنج Redragon M601 بإضاءة RGB ومستشعر بدقة 7200 DPI و6 أزرار قابلة للبرمجة",
                        Price = 745.00m,
                        StockQuantity = 30,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1605773527852-c546a8584ea3?w=500",
                        Brand = "Redragon"
                    },
                    new Product
                    {
                        ProductName = "Logitech G305 Wireless",
                        Description = "ماوس جيمنج لاسلكي Logitech G305 بتقنية LIGHTSPEED ومستشعر HERO بدقة 12000 DPI",
                        Price = 2250.00m,
                        StockQuantity = 12,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1629429408209-1f912961dbd8?w=500",
                        IsFeatured = true,
                        Brand = "Logitech"
                    },
                    new Product
                    {
                        ProductName = "Lenovo 300 Wired USB Mouse",
                        Description = "ماوس Lenovo 300 سلكي USB بتصميم مريح ودقة 1600 DPI - مثالي للمكتب والاستخدام اليومي",
                        Price = 199.00m,
                        StockQuantity = 50,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1613141411244-0e4ac259d217?w=500",
                        Brand = "Lenovo"
                    },
                    new Product
                    {
                        ProductName = "Logitech MX Master 3S",
                        Description = "ماوس Logitech MX Master 3S بتقنية Bluetooth ومستشعر بدقة 8000 DPI وشحن USB-C وتصميم ergonomic",
                        Price = 6760.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["Mouse"],
                        ImageUrl = "https://images.unsplash.com/photo-1600180758890-6b94519a8ba6?w=500",
                        Brand = "Logitech"
                    }
                });
            }

            // === KEYBOARD ===
            if (categoryDict.ContainsKey("Keyboard"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "Redragon K552 Kumara",
                        Description = "كيبورد جيمنج ميكانيكي Redragon K552 Kumara بمفاتيح Blue Switch وإضاءة RGB وتصميم TKL",
                        Price = 1550.00m,
                        StockQuantity = 20,
                        CategoryId = categoryDict["Keyboard"],
                        ImageUrl = "https://images.unsplash.com/photo-1595044426077-d36d9236d54a?w=500",
                        Brand = "Redragon"
                    },
                    new Product
                    {
                        ProductName = "Redragon K617 60% Gaming",
                        Description = "كيبورد جيمنج ميكانيكي Redragon K617 بحجم 60% ومفاتيح Red Switch وإضاءة RGB",
                        Price = 1600.00m,
                        StockQuantity = 18,
                        CategoryId = categoryDict["Keyboard"],
                        ImageUrl = "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=500",
                        IsFeatured = true,
                        Brand = "Redragon"
                    },
                    new Product
                    {
                        ProductName = "Corsair K100 RGB",
                        Description = "كيبورد جيمنج Corsair K100 RGB ميكانيكي بمفاتيح Cherry MX Speed وعجلة تحكم iCUE",
                        Price = 6999.00m,
                        StockQuantity = 6,
                        CategoryId = categoryDict["Keyboard"],
                        ImageUrl = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=500",
                        Brand = "Corsair"
                    },
                    new Product
                    {
                        ProductName = "Logitech G915 TKL Wireless",
                        Description = "كيبورد جيمنج Logitech G915 TKL لاسلكي بتقنية LIGHTSPEED ومفاتيح GL Tactile وتصميم منخفض",
                        Price = 5499.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["Keyboard"],
                        ImageUrl = "https://images.unsplash.com/photo-1541140532154-b024d705b90a?w=500",
                        IsFeatured = true,
                        Brand = "Logitech"
                    },
                    new Product
                    {
                        ProductName = "Redragon K552P RGB",
                        Description = "كيبورد جيمنج ميكانيكي Redragon K552P بمفاتيح Blue Switch وإضاءة RGB كاملة",
                        Price = 2525.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["Keyboard"],
                        ImageUrl = "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=500",
                        Brand = "Redragon"
                    }
                });
            }

            // === CPU ===
            if (categoryDict.ContainsKey("CPU"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "Intel Core i9-14900K",
                        Description = "معالج Intel Core i9-14900K الجيل الرابع عشر بـ 24 نواة (8P+16E) و32 خيط وتردد حتى 6.0GHz",
                        Price = 24999.00m,
                        StockQuantity = 5,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1555617778-02518510b9fa?w=500",
                        IsFeatured = true,
                        Brand = "Intel"
                    },
                    new Product
                    {
                        ProductName = "Intel Core i7-14700K",
                        Description = "معالج Intel Core i7-14700K الجيل الرابع عشر بـ 20 نواة (8P+12E) و28 خيط وتردد حتى 5.6GHz",
                        Price = 17700.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?w=500",
                        IsFeatured = true,
                        Brand = "Intel"
                    },
                    new Product
                    {
                        ProductName = "Intel Core i5-14600K",
                        Description = "معالج Intel Core i5-14600K الجيل الرابع عشر بـ 14 نواة (6P+8E) و20 خيط وتردد حتى 5.3GHz",
                        Price = 13000.00m,
                        StockQuantity = 12,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=500",
                        Brand = "Intel"
                    },
                    new Product
                    {
                        ProductName = "Intel Core i5-14400F",
                        Description = "معالج Intel Core i5-14400F بـ 10 نوى (6P+4E) و16 خيط وتردد حتى 4.7GHz - بدون رسومات مدمجة",
                        Price = 7900.00m,
                        StockQuantity = 15,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1592664474505-51c549ad15c5?w=500",
                        Brand = "Intel"
                    },
                    new Product
                    {
                        ProductName = "AMD Ryzen 5 7600X",
                        Description = "معالج AMD Ryzen 5 7600X بـ 6 نوى و12 خيط وتردد حتى 5.3GHz مع دعم DDR5",
                        Price = 8999.00m,
                        StockQuantity = 10,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1555617117-08d2e0295b82?w=500",
                        Brand = "AMD"
                    },
                    new Product
                    {
                        ProductName = "Intel Core i3-12100F",
                        Description = "معالج Intel Core i3-12100F الجيل الثاني عشر بـ 4 نوى و8 خيط وتردد حتى 4.3GHz - الخيار الاقتصادي",
                        Price = 3400.00m,
                        StockQuantity = 20,
                        CategoryId = categoryDict["CPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=500",
                        Brand = "Intel"
                    }
                });
            }

            // === GPU ===
            if (categoryDict.ContainsKey("GPU"))
            {
                products.AddRange(new[]
                {
                    new Product
                    {
                        ProductName = "PNY RTX 4070 Ti 12GB",
                        Description = "كارت شاشة PNY GeForce RTX 4070 Ti بذاكرة 12GB GDDR6X للجيمنج بأداء عالي",
                        Price = 40000.00m,
                        StockQuantity = 4,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1591488320449-011701bb6704?w=500",
                        IsFeatured = true,
                        Brand = "PNY"
                    },
                    new Product
                    {
                        ProductName = "PNY RTX 4070 OC 12GB",
                        Description = "كارت شاشة PNY GeForce RTX 4070 VERTO بذاكرة 12GB GDDR6X مع تبريد محسن",
                        Price = 29000.00m,
                        StockQuantity = 6,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=500",
                        IsFeatured = true,
                        Brand = "PNY"
                    },
                    new Product
                    {
                        ProductName = "GIGABYTE RTX 5060 Ti EAGLE 8GB",
                        Description = "كارت شاشة GIGABYTE GeForce RTX 5060 Ti EAGLE OC بذاكرة 8GB GDDR7 من الجيل الجديد",
                        Price = 23500.00m,
                        StockQuantity = 8,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1592664474505-51c549ad15c5?w=500",
                        Brand = "GIGABYTE"
                    },
                    new Product
                    {
                        ProductName = "PNY RTX 4060 Ti OC 16GB",
                        Description = "كارت شاشة PNY GeForce RTX 4060 Ti XLR8 بذاكرة 16GB GDDR6 للجيمنج 1440p",
                        Price = 24800.00m,
                        StockQuantity = 7,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1555618254-84e27ede0a68?w=500",
                        Brand = "PNY"
                    },
                    new Product
                    {
                        ProductName = "ZOTAC RTX 3050 8GB",
                        Description = "كارت شاشة ZOTAC GeForce RTX 3050 ECO بذاكرة 8GB GDDR6 - الخيار الاقتصادي للجيمنج",
                        Price = 11200.00m,
                        StockQuantity = 12,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1603481588273-2f908a9a7a1b?w=500",
                        Brand = "ZOTAC"
                    },
                    new Product
                    {
                        ProductName = "ZOTAC RTX 3050 6GB Low Profile",
                        Description = "كارت شاشة ZOTAC GeForce RTX 3050 Low Profile بذاكرة 6GB GDDR6 - للأجهزة المدمجة",
                        Price = 9500.00m,
                        StockQuantity = 10,
                        CategoryId = categoryDict["GPU"],
                        ImageUrl = "https://images.unsplash.com/photo-1587202372616-b43abea06c2a?w=500",
                        Brand = "ZOTAC"
                    }
                });
            }

            // Add all products to context
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
