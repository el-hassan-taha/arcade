using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Arcade.Data;
using Arcade.Data.Repositories;
using Arcade.Services;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------------
// Database Context
// --------------------------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------------------------------------------------------------
// Repositories (Dependency Injection)
// --------------------------------------------------------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// --------------------------------------------------------------------------
// Services (Dependency Injection)
// --------------------------------------------------------------------------
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// --------------------------------------------------------------------------
// Authentication (Dual Cookie-based: Customer and Admin)
// --------------------------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CustomerScheme";
    options.DefaultChallengeScheme = "CustomerScheme";
})
.AddCookie("CustomerScheme", options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = "Arcade.Customer";
})
.AddCookie("AdminScheme", options =>
{
    options.LoginPath = "/Admin/Login";
    options.LogoutPath = "/Admin/Logout";
    options.AccessDeniedPath = "/Admin/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = false;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = "Arcade.Admin";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOnly", policy =>
    {
        policy.AuthenticationSchemes.Add("CustomerScheme");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Customer");
    });

    options.AddPolicy("AdminOnly", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminScheme");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

// --------------------------------------------------------------------------
// MVC & Session
// --------------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// --------------------------------------------------------------------------
// Database Initialization (Auto-migrate & Seed)
// --------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// --------------------------------------------------------------------------
// Middleware Pipeline
// --------------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
