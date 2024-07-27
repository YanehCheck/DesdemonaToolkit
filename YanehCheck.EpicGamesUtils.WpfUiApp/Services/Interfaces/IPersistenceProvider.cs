namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface IPersistenceProvider {
    public string AccountId { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiry { get; set; }
    public DateTime LastItemFetch { get; set; }
    public void Save();
    public void Reset();
}