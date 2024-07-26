using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Properties;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class SettingsProvider : ISettingsProvider {
    public SettingsProvider(IOptions<UserOptions> userOptions) {
        if (userOptions.Value.ResetUserSettings) {
            Reset();
        }
    }

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