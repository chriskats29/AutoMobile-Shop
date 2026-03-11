using Microsoft.EntityFrameworkCore;
using ECommerceStore.Components;
using ECommerceStore.Data;
using ECommerceStore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure PostgreSQL Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<AdminStateService>();
builder.Services.AddScoped<CartStateService>();

var app = builder.Build();

// Apply migrations and seed data automatically
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    
    db.Database.Migrate();
    Console.WriteLine("Database migration completed successfully.");
    
    // Ensure default admin user exists with correct password
    try
    {
        var adminUser = db.AdminUsers.FirstOrDefault(u => u.Username == "admin");
        if (adminUser == null)
        {
            Console.WriteLine("Creating admin user...");
            var created = authService.CreateAdminUserAsync("admin", "admin123").GetAwaiter().GetResult();
            if (created)
            {
                Console.WriteLine("✓ Default admin user created successfully.");
                Console.WriteLine("  Username: admin");
                Console.WriteLine("  Password: admin123");
                
                // Verify the password works
                var testLogin = authService.ValidateCredentialsAsync("admin", "admin123").GetAwaiter().GetResult();
                if (testLogin != null)
                {
                    Console.WriteLine("✓ Password verification test passed!");
                }
                else
                {
                    Console.WriteLine("✗ WARNING: Password verification test failed!");
                }
            }
            else
            {
                Console.WriteLine("✗ ERROR: Could not create default admin user.");
            }
        }
        else
        {
            Console.WriteLine("Admin user found. Resetting password...");
            // Always reset password to ensure it's correct
            var reset = authService.ResetAdminPasswordAsync("admin", "admin123").GetAwaiter().GetResult();
            if (reset)
            {
                Console.WriteLine("✓ Admin password has been reset to: admin123");
                
                // Verify the password works
                var testLogin = authService.ValidateCredentialsAsync("admin", "admin123").GetAwaiter().GetResult();
                if (testLogin != null)
                {
                    Console.WriteLine("✓ Password verification test passed!");
                }
                else
                {
                    Console.WriteLine("✗ WARNING: Password verification test failed after reset!");
                }
            }
            else
            {
                Console.WriteLine("✗ ERROR: Could not reset admin password.");
            }
            Console.WriteLine("Default credentials: admin / admin123");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ ERROR setting up admin user: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }

    // Update product images if they still have placeholder URLs
    var products = db.Products.ToList();
    var imageUpdates = new Dictionary<int, string>
    {
        { 1, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 2, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 3, "https://images.pexels.com/photos/163140/road-marking-tire-asphalt-163140.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 4, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 5, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 6, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 7, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" },
        { 8, "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop" }
    };

    bool imagesUpdated = false;
    foreach (var product in products)
    {
        if (product.ImageUrl.Contains("placeholder") || product.ImageUrl.Contains("placehold.co"))
        {
            if (imageUpdates.ContainsKey(product.Id))
            {
                product.ImageUrl = imageUpdates[product.Id];
                product.UpdatedAt = DateTime.UtcNow;
                imagesUpdated = true;
            }
        }
    }

    if (imagesUpdated)
    {
        db.SaveChanges();
        Console.WriteLine("Product images updated successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database connection failed: {ex.Message}");
    Console.WriteLine("Please ensure PostgreSQL is running and the connection string in appsettings.json is correct.");
    Console.WriteLine("Connection string format: Host=localhost;Database=AutoMotiveShopDb;Username=postgres;Password=your_password");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
