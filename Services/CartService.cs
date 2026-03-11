using Microsoft.EntityFrameworkCore;
using ECommerceStore.Data;
using ECommerceStore.Models;

namespace ECommerceStore.Services;

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetCartItemsAsync(string sessionId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.SessionId == sessionId)
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync();
    }

    public async Task<int> GetCartItemCountAsync(string sessionId)
    {
        return await _context.CartItems
            .Where(c => c.SessionId == sessionId)
            .SumAsync(c => c.Quantity);
    }

    public async Task<decimal> GetCartTotalAsync(string sessionId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.SessionId == sessionId)
            .SumAsync(c => c.Product.Price * c.Quantity);
    }

    public async Task<CartItem> AddToCartAsync(string sessionId, int productId, int quantity = 1)
    {
        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            await _context.SaveChangesAsync();
            return existingItem;
        }

        var cartItem = new CartItem
        {
            SessionId = sessionId,
            ProductId = productId,
            Quantity = quantity,
            AddedAt = DateTime.UtcNow
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        return cartItem;
    }

    public async Task<bool> UpdateQuantityAsync(string sessionId, int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.SessionId == sessionId);

        if (cartItem == null)
            return false;

        if (quantity <= 0)
        {
            _context.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity = quantity;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromCartAsync(string sessionId, int cartItemId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.SessionId == sessionId);

        if (cartItem == null)
            return false;

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task ClearCartAsync(string sessionId)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }
}
