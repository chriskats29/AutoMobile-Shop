using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.Models;

public class CartItem
{
    public int Id { get; set; }

    public string SessionId { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
