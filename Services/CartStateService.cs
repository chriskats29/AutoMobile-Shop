namespace ECommerceStore.Services;

public class CartStateService
{
    private string _sessionId = Guid.NewGuid().ToString();
    public string SessionId => _sessionId;

    public int ItemCount { get; private set; }

    public event Action? OnChange;

    public void UpdateItemCount(int count)
    {
        ItemCount = count;
        NotifyStateChanged();
    }

    public void IncrementItemCount(int amount = 1)
    {
        ItemCount += amount;
        NotifyStateChanged();
    }

    public void ResetSession()
    {
        _sessionId = Guid.NewGuid().ToString();
        ItemCount = 0;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
