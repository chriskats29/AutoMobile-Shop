using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedAt", "LastLoginAt", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "$2a$11$rBLRHwnHQ4IG7kHYSB5lsuCzSGZM1y3m9YpYE7Ae0d1RKxjrXFKhO", "admin" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImageUrl", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Engine", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-performance synthetic motor oil for all engine types. Provides excellent protection and fuel efficiency.", "https://via.placeholder.com/300x300?text=Motor+Oil", "Premium Motor Oil 5W-30", 34.99m, 100, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Brakes", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium ceramic brake pads with low dust and noise. Fits most sedans and SUVs.", "https://via.placeholder.com/300x300?text=Brake+Pads", "Ceramic Brake Pads Set", 89.99m, 45, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Tires", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-quality all-season tire with excellent grip in wet and dry conditions.", "https://via.placeholder.com/300x300?text=Tire", "All-Season Tire 205/55R16", 129.99m, 60, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Lighting", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bright LED headlight bulbs with 6000K white light. Plug and play installation.", "https://via.placeholder.com/300x300?text=LED+Headlight", "LED Headlight Bulbs H11", 49.99m, 80, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Electrical", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reliable car battery with 600 cold cranking amps. 3-year warranty included.", "https://via.placeholder.com/300x300?text=Battery", "Car Battery 12V 600CCA", 159.99m, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "Engine", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-flow air filter for improved engine performance and fuel economy.", "https://via.placeholder.com/300x300?text=Air+Filter", "Air Filter Performance", 24.99m, 120, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "Exterior", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium beam wiper blades with durable rubber for streak-free wiping.", "https://via.placeholder.com/300x300?text=Wipers", "Windshield Wipers 22\"", 19.99m, 90, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "Engine", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Long-lasting iridium spark plugs for optimal ignition. Set of 4.", "https://via.placeholder.com/300x300?text=Spark+Plugs", "Spark Plugs Iridium Set", 39.99m, 70, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Username",
                table: "AdminUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_SessionId",
                table: "CartItems",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
