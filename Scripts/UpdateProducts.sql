-- Delete all existing products and related data
DELETE FROM OrderDetails;
DELETE FROM CartItems;
DELETE FROM Products;

-- Reset identity seed
DBCC CHECKIDENT ('Products', RESEED, 0);

-- Insert real products from Egyptian stores (elbadrgroupeg.store)

-- Category IDs from InitialCreate:
-- 1 = Laptop, 2 = Monitor, 3 = RAM, 4 = SSD, 5 = Mouse, 6 = Keyboard, 7 = CPU, 8 = GPU

-- ==================== MONITORS (CategoryId = 2) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('ASUS ROG Strix OLED XG27ACDMS 27" QHD 280Hz', 2, 32500, 15, 
'27" QHD 280Hz QD-OLED Gaming Monitor with 0.03ms Response Time, Neo Proximity Sensor, OLED Care Pro, G-SYNC Compatible, USB-C PD, DisplayHDR 400 True Black. The ultimate gaming display for competitive esports and immersive gaming experiences.',
'27" QHD 280Hz QD-OLED Gaming Monitor', NULL, 'ASUS', 'XG27ACDMS', 1, 1, GETDATE()),

('ASUS ROG Strix OLED XG32UCWMG 32" 4K', 2, 49500, 8,
'32" 4K TrueBlack Glossy WOLED Gaming Monitor with Dual Mode (4K@240Hz / FHD@480Hz), 0.03ms Response Time, G-SYNC Compatible, OLED Care Pro, USB-C PD, DisplayHDR 400 True Black. Premium 4K OLED gaming experience.',
'32" 4K 240Hz WOLED Gaming Monitor', NULL, 'ASUS', 'XG32UCWMG', 1, 1, GETDATE()),

('ASUS ROG Strix XG248QSG Ace 24.1" FHD 610Hz', 2, 45000, 5,
'24.1" FHD 610Hz Esports Gaming Monitor with Super TN Panel, 0.1ms Response Time, ELMB 2, G-SYNC Compatible, HDMI 2.1. Designed for professional esports with the fastest refresh rate available.',
'24.1" FHD 610Hz Esports Monitor', NULL, 'ASUS', 'XG248QSG', 1, 1, GETDATE()),

('ASUS TUF Gaming VG27AQL5A 27" QHD 210Hz', 2, 13200, 20,
'27" QHD Fast-IPS 210Hz(OC) Gaming Monitor with 0.3ms GTG, ELMB Sync, 95% DCI-P3. Excellent color accuracy and fast response for both gaming and content creation.',
'27" QHD 210Hz Fast-IPS Monitor', NULL, 'ASUS', 'VG27AQL5A', 1, 1, GETDATE()),

('Gigabyte GS27FC2 27" Curved 240Hz', 2, 7800, 25,
'27" Curved Gaming Monitor with Super Speed VA 1500R, Full HD 240Hz, 1ms response time, 125% sRGB. Great value curved gaming monitor with immersive experience.',
'27" Curved 240Hz VA Monitor', NULL, 'Gigabyte', 'GS27FC2', 1, 0, GETDATE());

-- ==================== RAM (CategoryId = 3) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('Kingston FURY Renegade RGB DDR5 48GB 6000MT/s', 3, 19500, 12,
'DDR5 48GB (1×48GB) 6000MT/s CL32 Desktop Memory Module. High-performance DDR5 RAM with RGB lighting and Intel XMP 3.0 support for extreme overclocking.',
'48GB DDR5-6000 RGB Memory', NULL, 'Kingston', 'KF560C32RSA-48', 1, 1, GETDATE()),

('G.Skill Trident Z5 RGB DDR5-6400 96GB', 3, 39000, 6,
'96GB (2×48GB) CL32-39-39-102 1.35V Gaming Memory Kit. Premium high-capacity DDR5 kit with stunning RGB lighting for enthusiast builds.',
'96GB DDR5-6400 RGB Kit', NULL, 'G.Skill', 'F5-6400J3239F48GX2-TZ5RK', 1, 1, GETDATE()),

('Kingston FURY Renegade RGB DDR5 96GB Kit', 3, 37500, 8,
'96 GB (2×48 GB) 6000 MT/s CL32 1.35V Desktop Gaming Memory Kit. Dual-channel DDR5 kit for maximum bandwidth and multitasking performance.',
'96GB DDR5-6000 RGB Kit', NULL, 'Kingston', 'KF560C32RSAK2-96', 1, 1, GETDATE()),

('ACER UD200 32GB DDR5 5600MHz', 3, 14500, 15,
'32 GB DDR5 5600 MHz UDIMM Desktop Memory Module with CL46, 288-Pin, 1.1V, On-Die ECC & PMIC Power Management. Reliable DDR5 memory at great value.',
'32GB DDR5-5600 UDIMM', NULL, 'Acer', 'UD200', 1, 0, GETDATE()),

('CORSAIR Vengeance RGB RS 16GB DDR4-3200', 3, 4500, 30,
'16GB (2x8GB) DDR4-3200 C16 Desktop Memory with XMP 2.0, 16-20-20-38, 1.35V, PC4-25600, iCUE RGB, anodized aluminum heatspreader. Great entry-level RGB memory kit.',
'16GB DDR4-3200 RGB Kit', NULL, 'Corsair', 'CMG16GX4M2E3200C16', 1, 0, GETDATE()),

('G.Skill Trident Z5 Royal 48GB DDR5-8400', 3, 24500, 5,
'48GB (2x24GB) DDR5-8400 CL40-52-52-134 1.40V Dual Channel Desktop Memory Kit with Intel XMP 3.0, Crystalline RGB Light Bar, Electroplated Aluminum Heatspreaders.',
'48GB DDR5-8400 Royal RGB Kit', NULL, 'G.Skill', 'F5-8400J4052G24GX2-TR5G', 1, 1, GETDATE()),

('G.Skill Trident Z5 RGB 32GB DDR5-5600', 3, 15500, 18,
'32GB (2x16GB) DDR5 5600MHz CL40 Desktop Memory Kit with Intel XMP 3.0, 40-40-40-89 Latency, 1.20V, Unbuffered Non-ECC, Silver Heatspreader.',
'32GB DDR5-5600 RGB Kit', NULL, 'G.Skill', 'F5-5600J4040C16gx2-TZ5RS', 1, 0, GETDATE()),

('Kingston FURY Renegade RGB 64GB DDR5-6400', 3, 32000, 10,
'64GB (2x32GB) DDR5-6400 CL32 288-Pin DIMM Kit with Dual Rank 2Rx8, RGB Lighting with Infrared Sync, Intel XMP 3.0, 1.4V, Silver/Black Heatspreaders.',
'64GB DDR5-6400 RGB Kit', NULL, 'Kingston', 'KF564C32RSAK2-64', 1, 1, GETDATE());

-- ==================== SSD (CategoryId = 4) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('SanDisk 2TB USB 3.2 Gen 2 Portable SSD', 4, 7500, 20,
'Up to 800MB/s Read Speed, Rugged Drop Protection (2m), USB-C & USB-A Compatible. Portable storage solution for content creators and professionals.',
'2TB Portable SSD 800MB/s', NULL, 'SanDisk', 'SDSSDE30-2T00-G26', 1, 1, GETDATE()),

('WD_BLACK SN8100 4TB NVMe SSD PCIe 5.0', 4, 28500, 6,
'PCIe 5.0 x4/Gen5, M.2 2280, Up to 14,900 MB/s Read & 14,000 MB/s Write. The fastest consumer NVMe SSD for extreme performance.',
'4TB PCIe 5.0 NVMe SSD', NULL, 'WD', 'WDS400T1X0M-00CMT0', 1, 1, GETDATE()),

('WD_BLACK SN8100 2TB NVMe SSD PCIe 5.0', 4, 17500, 10,
'PCIe 5.0 x4 (Gen 5), M.2 2280, Up to 14,900 MB/s Read & 14,000 MB/s Write. Next-gen storage speed for gaming and content creation.',
'2TB PCIe 5.0 NVMe SSD', NULL, 'WD', 'WDS200T1X0M-00CMT0', 1, 1, GETDATE()),

('WD_BLACK SN8100 1TB NVMe SSD PCIe 5.0', 4, 10500, 15,
'PCIe 5.0 x4 (Gen 5), M.2 2280, Up to 14,900 MB/s Read & 11,000 MB/s Write. High-speed NVMe storage at accessible capacity.',
'1TB PCIe 5.0 NVMe SSD', NULL, 'WD', 'WDS100T1X0M-00CMT0', 1, 0, GETDATE()),

('SanDisk Extreme Portable SSD 2TB', 4, 9000, 12,
'For PlayStation 5 & PC with USB-C (USB 3.2 Gen 2), Up to 1000 MB/s Read Speed, IP65 Rugged Design. Perfect for console gaming expansion.',
'2TB Extreme Portable SSD', NULL, 'SanDisk', 'SDSSDE62P-2T00-G25', 1, 0, GETDATE()),

('Kingston XS2000 Portable SSD 2TB', 4, 14000, 8,
'USB 3.2 Gen 2x2, up to 2000MB/s read/write, USB-C, includes rubber sleeve. Ultra-fast portable storage with pocket-sized design.',
'2TB Portable SSD 2000MB/s', NULL, 'Kingston', 'SXS2000/2000G', 1, 0, GETDATE()),

('SanDisk 1TB Extreme Portable SSD', 4, 6500, 25,
'Up to 1050MB/s, USB-C, USB 3.2 Gen 2, IP65 Water and Dust Resistance. Durable and fast portable storage solution.',
'1TB Extreme Portable SSD', NULL, 'SanDisk', 'SDSSDE61-1T00-G25', 1, 0, GETDATE()),

('SanDisk 1TB Portable SSD', 4, 4000, 30,
'Up to 800MB/s, USB-C, USB 3.2 Gen 2. Affordable portable storage with reliable performance.',
'1TB Portable SSD 800MB/s', NULL, 'SanDisk', 'SDSSDE30-1T00-G26', 1, 0, GETDATE()),

('Western Digital 4TB WD Blue SA510 SATA SSD', 4, 13500, 10,
'SATA III 6 Gb/s, 2.5"/7mm, Up to 560 MB/s. High-capacity SATA SSD for system upgrades.',
'4TB SATA SSD', NULL, 'WD', 'WDS400T3B0A', 1, 0, GETDATE()),

('Western Digital 2TB WD Blue SA510 SATA SSD', 4, 6900, 20,
'SATA III 6 Gb/s, 2.5"/7mm, Up to 560 MB/s. Reliable SATA SSD for everyday computing.',
'2TB SATA SSD', NULL, 'WD', 'WDS200T3B0A', 1, 0, GETDATE()),

('SanDisk 4TB Extreme PRO Portable SSD USB4', 4, 23500, 5,
'Up to 3800 MB/s Read and 3700MB/s Write, USB-C, USB 3.2, Backwards Compatible, IP65 Water and Dust Resistance. Professional-grade portable storage.',
'4TB Extreme PRO USB4 SSD', NULL, 'SanDisk', 'SDSSDE82-4T00-G25', 1, 1, GETDATE()),

('SanDisk 2TB Extreme PRO Portable SSD USB4', 4, 16000, 8,
'Up to 3800 MB/s Read and 3700MB/s Write, USB-C, USB 3.2, Backwards Compatible, IP65 Water and Dust Resistance. Fast professional portable storage.',
'2TB Extreme PRO USB4 SSD', NULL, 'SanDisk', 'SDSSDE82-2T00-G25', 1, 1, GETDATE());

-- ==================== CPU (CategoryId = 7) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('AMD Ryzen 5 3600 3.6 GHz AM4', 7, 3200, 25,
'6-Core 12-Thread Desktop Processor with Socket AM4, 65W TDP, 3.6GHz Base / 4.2GHz Boost Clock. Great value CPU for gaming and productivity.',
'6-Core 3.6GHz AM4 Processor', NULL, 'AMD', '100-100000031BOX', 1, 1, GETDATE()),

('AMD Ryzen 5 5500 6-Core AM4', 7, 3600, 20,
'Ryzen 5 5000 Series 6-Core Socket AM4 65W Desktop Processor. Excellent mid-range CPU with Zen 3 architecture.',
'6-Core 65W AM4 Processor', NULL, 'AMD', '100-100000457BOX', 1, 1, GETDATE()),

('AMD Ryzen 7 5800X 8-Core AM4', 7, 6500, 12,
'8-Core 16-Thread Desktop Processor with 3.8 GHz Base Clock, Socket AM4, 105W TDP. High-performance CPU for gaming and content creation.',
'8-Core 3.8GHz AM4 Processor', NULL, 'AMD', '100-100000063WOF', 1, 1, GETDATE()),

('AMD Ryzen 9 5900X 12-Core AM4', 7, 9500, 8,
'12-Core 24-Thread Desktop Processor with 3.7 GHz Base Clock, Socket AM4, 105W TDP. Enthusiast-grade CPU for demanding workloads.',
'12-Core 3.7GHz AM4 Processor', NULL, 'AMD', '100-100000061WOF', 1, 1, GETDATE()),

('AMD Ryzen 9 5950X 16-Core AM4', 7, 13500, 5,
'16-Core 32-Thread Desktop Processor with 3.4 GHz Base Clock, Socket AM4, 105W TDP. The ultimate AM4 processor for professionals.',
'16-Core 3.4GHz AM4 Processor', NULL, 'AMD', '100-100000059WOF', 1, 1, GETDATE()),

('Intel Core i5-14600K Unlocked', 7, 12000, 10,
'14th Gen Intel Core i5, 14 Cores (6 P-cores + 8 E-cores), 20 Threads, up to 5.3 GHz, LGA 1700 Socket, 125W TDP. Great for gaming and multitasking.',
'14-Core Unlocked LGA1700 CPU', NULL, 'Intel', 'BX8071514600K', 1, 1, GETDATE()),

('Intel Core i7-14700K Unlocked', 7, 17500, 8,
'14th Gen Intel Core i7, 20 Cores (8 P-cores + 12 E-cores), 28 Threads, up to 5.6 GHz, LGA 1700 Socket, 125W TDP. High-performance gaming and productivity.',
'20-Core Unlocked LGA1700 CPU', NULL, 'Intel', 'BX8071514700K', 1, 1, GETDATE()),

('Intel Core i9-14900K Unlocked', 7, 25000, 5,
'14th Gen Intel Core i9, 24 Cores (8 P-cores + 16 E-cores), 32 Threads, up to 6.0 GHz, LGA 1700 Socket, 125W TDP. Flagship gaming and workstation CPU.',
'24-Core Unlocked LGA1700 CPU', NULL, 'Intel', 'BX8071514900K', 1, 1, GETDATE());

-- ==================== KEYBOARD (CategoryId = 6) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('DAREU A75 HE Wired Aluminum Gaming Keyboard Black', 6, 7500, 15,
'Magnetic Switches, 8000Hz Polling Rate, 0.02mm Actuation, 0.2ms Low Latency, 220-Grit Aluminum Cover, Ultra-Fast Response, Premium Build for Competitive Gaming.',
'75% Magnetic Switch Keyboard', NULL, 'DAREU', 'A75-HE-BLK', 1, 1, GETDATE()),

('DAREU A75 HE Wired Aluminum Gaming Keyboard Gold', 6, 7500, 12,
'Magnetic Switches, 8000Hz Polling Rate, 0.02mm Actuation, 0.2ms Low Latency, 220-Grit Aluminum Cover, Ultra-Fast Response, Premium Build for Competitive Gaming.',
'75% Magnetic Switch Keyboard', NULL, 'DAREU', 'A75-HE-GLD', 1, 1, GETDATE()),

('DAREU EK75RT 8K Wired Aluminum Gaming Keyboard', 6, 3950, 20,
'75% Layout, Magnetic Switches, 8K Hz Polling Rate, Adjustable Actuation & Rapid Trigger, Metal Volume Knob, South-Facing RGB, Solid Aluminum Alloy Build.',
'75% Layout 8K Hz Keyboard', NULL, 'DAREU', 'EK75RT', 1, 0, GETDATE()),

('Logitech G Pro X Mechanical Gaming Keyboard', 6, 5500, 18,
'Tenkeyless Design, Pro-grade Switches, LIGHTSYNC RGB, Detachable Cable, Portable Design. Designed with and for esports professionals.',
'TKL Mechanical Gaming Keyboard', NULL, 'Logitech', 'G-PRO-X', 1, 0, GETDATE()),

('Razer BlackWidow V4 75%', 6, 8500, 10,
'Hot-swappable Mechanical Gaming Keyboard, Razer Orange Switches, Chroma RGB, Aluminum Top Case, Dedicated Media Keys.',
'75% Hot-swap Mechanical Keyboard', NULL, 'Razer', 'RZ03-05000', 1, 1, GETDATE()),

('SteelSeries Apex Pro TKL Wireless', 6, 11000, 8,
'Adjustable Actuation Switches, OLED Smart Display, Bluetooth & 2.4GHz, 40-hour Battery, Aircraft-grade Aluminum Frame.',
'TKL Wireless Gaming Keyboard', NULL, 'SteelSeries', 'APEX-PRO-TKL-W', 1, 1, GETDATE());

-- ==================== MOUSE (CategoryId = 5) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('Logitech G Pro X Superlight 2', 5, 7500, 15,
'Wireless Gaming Mouse, 32K HERO 2 Sensor, 95-hour Battery, LIGHTFORCE Hybrid Switches, 60g Ultralight Design. The choice of esports professionals.',
'60g Wireless Gaming Mouse', NULL, 'Logitech', 'G-PRO-X-SL2', 1, 1, GETDATE()),

('Razer DeathAdder V3 Pro', 5, 6500, 18,
'Wireless Ergonomic Gaming Mouse, 30K Optical Sensor, 90-hour Battery, 63g Weight, Focus Pro Technology.',
'63g Ergonomic Wireless Mouse', NULL, 'Razer', 'RZ01-04630', 1, 1, GETDATE()),

('SteelSeries Prime Mini Wireless', 5, 4500, 20,
'Ultra-lightweight Wireless Gaming Mouse, TrueMove Air Sensor, 100-hour Battery, Prestige OM Switches, 73g Weight.',
'73g Mini Wireless Mouse', NULL, 'SteelSeries', 'PRIME-MINI-W', 1, 0, GETDATE()),

('Glorious Model O 2 Wireless', 5, 5500, 15,
'Ultralight Gaming Mouse, 26K BAMF 2.0 Sensor, 95-hour Battery, SUPERGLIDES, 59g Honeycomb Design.',
'59g Ultralight Wireless Mouse', NULL, 'Glorious', 'GLO-MS-OW2', 1, 0, GETDATE()),

('Pulsar X2 V2 Mini Wireless', 5, 4800, 12,
'Competitive Gaming Mouse, PAW3395 Sensor, 70-hour Battery, Optical Switches, 52g Ultralight.',
'52g Mini Gaming Mouse', NULL, 'Pulsar', 'X2V2-MINI', 1, 1, GETDATE()),

('Zowie EC2-CW Wireless', 5, 6000, 10,
'Ergonomic Esports Mouse, 3370 Sensor, Wireless with 24-step Scroll, 77g Weight, Designed by Pro Players.',
'77g Ergonomic Esports Mouse', NULL, 'Zowie', 'EC2-CW', 1, 0, GETDATE());

-- ==================== GPU (CategoryId = 8) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('NVIDIA GeForce RTX 4090 Founders Edition', 8, 95000, 3,
'24GB GDDR6X, 16384 CUDA Cores, 2.52 GHz Boost Clock, PCIe 4.0, DLSS 3, Ray Tracing. The ultimate gaming and AI graphics card.',
'24GB GDDR6X Flagship GPU', NULL, 'NVIDIA', 'RTX4090-FE', 1, 1, GETDATE()),

('NVIDIA GeForce RTX 4080 Super', 8, 65000, 5,
'16GB GDDR6X, 10240 CUDA Cores, 2.55 GHz Boost Clock, PCIe 4.0, DLSS 3, Ray Tracing. High-end gaming performance.',
'16GB GDDR6X Gaming GPU', NULL, 'NVIDIA', 'RTX4080S', 1, 1, GETDATE()),

('NVIDIA GeForce RTX 4070 Ti Super', 8, 45000, 8,
'16GB GDDR6X, 8448 CUDA Cores, 2.61 GHz Boost Clock, PCIe 4.0, DLSS 3, Ray Tracing. Excellent 4K gaming performance.',
'16GB GDDR6X Gaming GPU', NULL, 'NVIDIA', 'RTX4070TiS', 1, 1, GETDATE()),

('NVIDIA GeForce RTX 4070 Super', 8, 32000, 10,
'12GB GDDR6X, 7168 CUDA Cores, 2.48 GHz Boost Clock, PCIe 4.0, DLSS 3, Ray Tracing. Great value for 1440p gaming.',
'12GB GDDR6X Gaming GPU', NULL, 'NVIDIA', 'RTX4070S', 1, 0, GETDATE()),

('NVIDIA GeForce RTX 4060 Ti', 8, 22000, 15,
'8GB GDDR6, 4352 CUDA Cores, 2.54 GHz Boost Clock, PCIe 4.0, DLSS 3, Ray Tracing. Solid 1080p and 1440p gaming.',
'8GB GDDR6 Gaming GPU', NULL, 'NVIDIA', 'RTX4060Ti', 1, 0, GETDATE()),

('AMD Radeon RX 7900 XTX', 8, 55000, 6,
'24GB GDDR6, 6144 Stream Processors, 2.5 GHz Boost Clock, PCIe 4.0, FSR 3, Ray Tracing. AMD''s flagship gaming GPU.',
'24GB GDDR6 Gaming GPU', NULL, 'AMD', 'RX7900XTX', 1, 1, GETDATE()),

('AMD Radeon RX 7900 XT', 8, 42000, 8,
'20GB GDDR6, 5376 Stream Processors, 2.4 GHz Boost Clock, PCIe 4.0, FSR 3, Ray Tracing. Excellent performance-per-dollar.',
'20GB GDDR6 Gaming GPU', NULL, 'AMD', 'RX7900XT', 1, 0, GETDATE()),

('AMD Radeon RX 7800 XT', 8, 28000, 12,
'16GB GDDR6, 3840 Stream Processors, 2.43 GHz Boost Clock, PCIe 4.0, FSR 3, Ray Tracing. Great for 1440p gaming.',
'16GB GDDR6 Gaming GPU', NULL, 'AMD', 'RX7800XT', 1, 0, GETDATE());

-- ==================== LAPTOP (CategoryId = 1) ====================
INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, ShortDescription, ImageUrl, Brand, SKU, IsActive, IsFeatured, CreatedAt)
VALUES 
('ASUS ROG Strix G16 Gaming Laptop', 1, 75000, 5,
'16" QHD+ 240Hz, Intel Core i9-14900HX, NVIDIA RTX 4070 8GB, 32GB DDR5, 1TB NVMe SSD, ROG Intelligent Cooling, Per-Key RGB Keyboard.',
'16" i9/RTX4070 Gaming Laptop', NULL, 'ASUS', 'G614JU', 1, 1, GETDATE()),

('ASUS ROG Zephyrus G14 2024', 1, 95000, 4,
'14" QHD+ 165Hz OLED, AMD Ryzen 9 8945HS, NVIDIA RTX 4090 8GB, 32GB DDR5X, 1TB NVMe SSD, AniMe Matrix Display.',
'14" R9/RTX4090 OLED Laptop', NULL, 'ASUS', 'GA403UI', 1, 1, GETDATE()),

('MSI Raider GE78 HX 14V', 1, 120000, 3,
'17" UHD+ 120Hz Mini LED, Intel Core i9-14900HX, NVIDIA RTX 4090 16GB, 64GB DDR5, 2TB NVMe SSD, Cooler Boost 5 Thermal.',
'17" i9/RTX4090 Gaming Laptop', NULL, 'MSI', 'GE78HX-14V', 1, 1, GETDATE()),

('Lenovo Legion Pro 7i Gen 9', 1, 85000, 4,
'16" WQXGA 240Hz, Intel Core i9-14900HX, NVIDIA RTX 4080 12GB, 32GB DDR5, 1TB NVMe SSD, LA-3 Coldfront Thermal.',
'16" i9/RTX4080 Gaming Laptop', NULL, 'Lenovo', 'LEGION-PRO-7I', 1, 1, GETDATE()),

('ASUS TUF Gaming A15', 1, 42000, 10,
'15.6" FHD 144Hz, AMD Ryzen 7 7735HS, NVIDIA RTX 4060 8GB, 16GB DDR5, 512GB NVMe SSD, MIL-STD-810H Durability.',
'15.6" R7/RTX4060 Gaming Laptop', NULL, 'ASUS', 'FA507NV', 1, 0, GETDATE()),

('HP Victus 16 Gaming Laptop', 1, 35000, 12,
'16.1" FHD 144Hz, Intel Core i5-13500H, NVIDIA RTX 4050 6GB, 16GB DDR5, 512GB NVMe SSD, Enhanced Thermal Design.',
'16.1" i5/RTX4050 Gaming Laptop', NULL, 'HP', 'VICTUS-16', 1, 0, GETDATE()),

('Dell G15 Gaming Laptop', 1, 38000, 8,
'15.6" FHD 165Hz, Intel Core i7-13650HX, NVIDIA RTX 4060 8GB, 16GB DDR5, 512GB NVMe SSD, Alienware-inspired Thermal Design.',
'15.6" i7/RTX4060 Gaming Laptop', NULL, 'Dell', 'G15-5530', 1, 0, GETDATE()),

('Acer Nitro V 15 Gaming', 1, 32000, 15,
'15.6" FHD 144Hz, AMD Ryzen 5 7535HS, NVIDIA RTX 4050 6GB, 16GB DDR5, 512GB NVMe SSD, NitroSense Control Center.',
'15.6" R5/RTX4050 Gaming Laptop', NULL, 'Acer', 'NITRO-V-15', 1, 0, GETDATE());

-- Print summary
SELECT 'Products inserted successfully!' AS Message;
SELECT CategoryId, COUNT(*) AS ProductCount FROM Products GROUP BY CategoryId;
SELECT COUNT(*) AS TotalProducts FROM Products;
