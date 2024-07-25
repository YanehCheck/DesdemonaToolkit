using YanehCheck.EpicGamesUtils.WpfUiApp.Properties;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class SettingsProvider : ISettingsProvider {
    public string AccessToken {
        get => Settings.Default.AccessToken;
        set => Settings.Default.AccessToken = value;
    }

    public DateTime AccessTokenExpiry {
        get => Settings.Default.AccessTokenExpiry;
        set => Settings.Default.AccessTokenExpiry = value;
    }

    public void Save() => Settings.Default.Save();
    public void Reset() => Settings.Default.Reset();
}