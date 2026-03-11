using ECommerceStore.Models;

namespace ECommerceStore.Services;

public class AdminStateService
{
    public AdminUser? CurrentAdmin { get; private set; }
    public bool IsAuthenticated => CurrentAdmin != null;

    public event Action? OnChange;

    public void Login(AdminUser admin)
    {
        CurrentAdmin = admin;
        NotifyStateChanged();
    }

    public void Logout()
    {
        CurrentAdmin = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
