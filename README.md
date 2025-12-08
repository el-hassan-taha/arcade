# 🎮 Arcade Electronics

A modern, feature-rich e-commerce platform for electronics and gaming products built with ASP.NET Core MVC.

## ✨ Features

- **🛒 Shopping Cart** - Full-featured shopping cart with real-time updates
- **👤 User Authentication** - Secure login and registration system
- **🎨 Modern UI/UX** - Beautiful cyberpunk-themed interface with light/dark mode
- **📱 Responsive Design** - Works seamlessly on all devices
- **🔍 Advanced Search** - Real-time product search with autocomplete
- **📦 Order Management** - Complete order tracking and history
- **👨‍💼 Admin Dashboard** - Comprehensive admin panel for product and order management
- **💳 Checkout System** - Smooth and secure checkout process
- **🎯 Product Categories** - Organized product browsing by categories
- **✨ Animations** - Smooth transitions and interactive effects throughout

## 🚀 Technologies Used

- **Backend**: ASP.NET Core MVC (.NET 10.0)
- **Database**: Entity Framework Core with SQL Server
- **Frontend**: HTML5, CSS3, JavaScript
- **Styling**: Bootstrap 5, Custom CSS with CSS Variables
- **Icons**: Font Awesome
- **Authentication**: ASP.NET Core Identity

## 🎯 Key Highlights

- **Dual Theme Support**: Seamless light/dark mode toggle
- **Advanced Animations**: Scroll-triggered reveals, parallax effects, and micro-interactions
- **Product Management**: Full CRUD operations for products with image support
- **Order Processing**: Complete order lifecycle management
- **User Profiles**: Personalized user accounts with order history
- **Admin Controls**: Comprehensive dashboard for inventory and order management

## 📦 Installation

1. Clone the repository:

```bash
git clone https://github.com/el-hassan-taha/arcade.git
cd arcade
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Update the database connection string in `appsettings.json`

4. Apply migrations:

```bash
dotnet ef database update
```

5. Run the application:

```bash
dotnet run
```

6. Open your browser and navigate to `https://localhost:5197`

## 🗄️ Database Setup

The application uses Entity Framework Core with SQL Server. The database will be automatically created when you run migrations.

Default seeded data includes:

- Product categories (Laptop, Monitor, RAM, SSD, Mouse, Keyboard, CPU, GPU)
- Sample products with images
- Admin and user accounts

## 👥 Collaborators

This project was developed by:

- [@el-hassan-taha](https://github.com/el-hassan-taha)
- [@kareemkoko1234](https://github.com/kareemkoko1234)
- [@Mahmoud-Megahed1](https://github.com/Mahmoud-Megahed1)
- [@osamahm1](https://github.com/osamahm1)
- [@Ra-7-eem](https://github.com/Ra-7-eem)
- [@Ahmedabokhyal](https://github.com/Ahmedabokhyal)

## 📁 Project Structure

```
Arcade/
├── Controllers/         # MVC Controllers
├── Models/             # Data models
├── Views/              # Razor views
├── ViewModels/         # View models
├── Data/               # Database context and repositories
├── Services/           # Business logic services
├── Migrations/         # EF Core migrations
├── wwwroot/            # Static files (CSS, JS, images)
│   ├── css/           # Stylesheets
│   ├── js/            # JavaScript files
│   └── images/        # Product and UI images
└── Properties/         # Launch settings
```

## 🎨 Features Overview

### For Customers

- Browse products by category
- Search products with real-time suggestions
- Add items to cart
- Secure checkout process
- Order history tracking
- Profile management
- Light/dark theme toggle

### For Administrators

- Dashboard with statistics
- Product inventory management
- Order processing and status updates
- User management
- Category management
- Sales analytics

## 🔐 Security

- Password hashing with ASP.NET Core Identity
- Secure authentication and authorization
- CSRF protection
- Input validation and sanitization
- SQL injection prevention through EF Core

## 🌟 UI/UX Highlights

- **Cyberpunk Theme**: Modern gradient-based design with purple and cyan accents
- **Smooth Animations**: Scroll reveals, hover effects, and transitions
- **Interactive Elements**: Ripple effects, floating particles, and parallax backgrounds
- **Responsive Layout**: Mobile-first design approach
- **Accessibility**: Keyboard navigation and screen reader support

## 📄 License

This project is open source and available for educational purposes.

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the issues page.

## 📧 Contact

For questions or feedback, please reach out to the project maintainers.
