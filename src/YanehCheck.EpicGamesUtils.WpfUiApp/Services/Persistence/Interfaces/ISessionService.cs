namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;

public interface ISessionService
{
    public bool IsAuthenticated { get; }
    public string? AccountId { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiry { get; set; }
    public bool IsItemDataFetched { get; set; }
    public string? DisplayName { get; set; }
    bool IsStyleDataFetched { get; set; }
}