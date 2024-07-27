namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface ISessionService {
    public string? AccountId { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiry { get; set; }
    public bool IsItemDataFetched { get; set; }
    public string? DisplayName { get; set; }
}