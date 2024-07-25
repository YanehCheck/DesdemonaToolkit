namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface ISettingsProvider {
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiry { get; set; }
    public void Save();
    public void Reset();
}