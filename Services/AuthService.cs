using Microsoft.EntityFrameworkCore;
using ECommerceStore.Data;
using ECommerceStore.Models;

namespace ECommerceStore.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminUser?> ValidateCredentialsAsync(string username, string password)
    {
        try
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine($"[AuthService] User '{username}' not found in database.");
                return null;
            }

            // Verify password
            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            
            if (!isValid)
            {
                Console.WriteLine($"[AuthService] Password verification failed for user '{username}'.");
                return null;
            }

            Console.WriteLine($"[AuthService] Login successful for user '{username}'.");
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] Exception during login: {ex.Message}");
            Console.WriteLine($"[AuthService] Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    public async Task<bool> CreateAdminUserAsync(string username, string password)
    {
        var existingUser = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser != null)
            return false;

        var user = new AdminUser
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _context.AdminUsers.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangePasswordAsync(string username, string currentPassword, string newPassword)
    {
        var user = await ValidateCredentialsAsync(username, currentPassword);

        if (user == null)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ResetAdminPasswordAsync(string username, string newPassword)
    {
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> VerifyAdminPasswordAsync(string username, string password)
    {
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
