using Microsoft.EntityFrameworkCore;
using ECommerceStore.Models;

namespace ECommerceStore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.HasIndex(e => e.Category);
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.SessionId);
        });

        // Static seed date to avoid EF Core PendingModelChangesWarning
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed default admin user (password: admin123)
        modelBuilder.Entity<AdminUser>().HasData(new AdminUser
        {
            Id = 1,
            Username = "admin",
            // BCrypt hash of "admin123" - Generated with BCrypt.Net
            PasswordHash = "$2a$11$rBLRHwnHQ4IG7kHYSB5lsuCzSGZM1y3m9YpYE7Ae0d1RKxjrXFKhO",
            CreatedAt = seedDate
        });

        // Seed automotive products with realistic product images from Pexels
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Premium Motor Oil 5W-30",
                Description = "High-performance synthetic motor oil for all engine types. Provides excellent protection and fuel efficiency.",
                Price = 34.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Engine",
                StockQuantity = 100,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 2,
                Name = "Ceramic Brake Pads Set",
                Description = "Premium ceramic brake pads with low dust and noise. Fits most sedans and SUVs.",
                Price = 89.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Brakes",
                StockQuantity = 45,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 3,
                Name = "All-Season Tire 205/55R16",
                Description = "High-quality all-season tire with excellent grip in wet and dry conditions.",
                Price = 129.99m,
                ImageUrl = "https://images.pexels.com/photos/163140/road-marking-tire-asphalt-163140.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Tires",
                StockQuantity = 60,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 4,
                Name = "LED Headlight Bulbs H11",
                Description = "Bright LED headlight bulbs with 6000K white light. Plug and play installation.",
                Price = 49.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Lighting",
                StockQuantity = 80,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 5,
                Name = "Car Battery 12V 600CCA",
                Description = "Reliable car battery with 600 cold cranking amps. 3-year warranty included.",
                Price = 159.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Electrical",
                StockQuantity = 25,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 6,
                Name = "Air Filter Performance",
                Description = "High-flow air filter for improved engine performance and fuel economy.",
                Price = 24.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Engine",
                StockQuantity = 120,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 7,
                Name = "Windshield Wipers 22\"",
                Description = "Premium beam wiper blades with durable rubber for streak-free wiping.",
                Price = 19.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Exterior",
                StockQuantity = 90,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 8,
                Name = "Spark Plugs Iridium Set",
                Description = "Long-lasting iridium spark plugs for optimal ignition. Set of 4.",
                Price = 39.99m,
                ImageUrl = "https://images.pexels.com/photos/3802508/pexels-photo-3802508.jpeg?auto=compress&cs=tinysrgb&w=500&h=500&fit=crop",
                Category = "Engine",
                StockQuantity = 70,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );
    }
}
