# ECommerce Store

A mini Blazor Server application for e-commerce store management built with C#, HTML/CSS, and PostgreSQL.

## Features

### Client Features
- Browse products on the homepage
- Filter products by category
- View detailed product information
- Add products to shopping cart
- View and manage shopping cart

### Admin Features
- Secure admin login with username/password authentication
- Full product management (Create, Read, Update, Delete)
- View all products in a table format
- Add new products with images, categories, and pricing
- Edit existing product details
- Delete products

## Technology Stack

- **Framework**: Blazor Server (.NET 9)
- **Language**: C#
- **Database**: PostgreSQL with Entity Framework Core
- **Frontend**: HTML/CSS with component-scoped styles
- **Authentication**: Custom admin authentication with BCrypt password hashing

## Prerequisites

- .NET 9 SDK
- PostgreSQL Server (running on localhost:5432)

## Getting Started

### 1. Database Setup

Create a PostgreSQL database and update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ecommercestore;Username=postgres;Password=postgres"
}
```

### 2. Run the Application

```bash
dotnet run
```

The application will automatically apply migrations and seed initial data on startup.


### Default Admin Credentials

- **Username**: admin
- **Password**: admin123

> ⚠️ **Important**: Change the default admin password after first login in a production environment.

## Project Structure

```
ECommerceStore/
├── Components/
│   ├── Layout/           # Main layout and navigation
│   └── Pages/
│       ├── Admin/        # Admin dashboard and login
│       ├── Home.razor    # Product listing
│       ├── ProductDetails.razor
│       └── Cart.razor
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── Product.cs
│   ├── AdminUser.cs
│   └── CartItem.cs
├── Services/
│   ├── ProductService.cs
│   ├── AuthService.cs
│   ├── CartService.cs
│   ├── AdminStateService.cs
│   └── CartStateService.cs
└── Migrations/
```

## Sample Data

The application seeds the following sample products:
- Wireless Headphones (Electronics) - $79.99
- Running Shoes (Sports) - $129.99
- Coffee Maker (Home & Kitchen) - $49.99

## License

MIT License

