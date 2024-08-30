using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Properties;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence;

public class PersistenceProvider : IPersistenceProvider
{
    public PersistenceProvider(IOptions<UserOptions> userOptions)
    {
        if (userOptions.Value.ResetUserSettings)
        {
            Reset();
        }
    }
    public string AccountId
    {
        get => Settings.Default.AccountId;
        set => Settings.Default.AccountId = value;
    }

    public string AccessToken
    {
        get => Settings.Default.AccessToken;
        set => Settings.Default.AccessToken = value;
    }

    public DateTime AccessTokenExpiry
    {
        get => Settings.Default.AccessTokenExpiry;
        set => Settings.Default.AccessTokenExpiry = value;
    }

    public string DisplayName
    {
        get => Settings.Default.DisplayName;
        set => Settings.Default.DisplayName = value;
    }

    public FetchStatus ItemFetchStatus {
        get => (FetchStatus) Settings.Default.ItemFetchStatus;
        set => Settings.Default.ItemFetchStatus = (int) value;
    }


    public FetchStatus StyleFetchStatus {
        get => (FetchStatus) Settings.Default.StyleFetchStatus;
        set => Settings.Default.StyleFetchStatus = (int) value;
    }

    public DateTime LastItemFetch {
        get => Settings.Default.LastItemFetch;
        set => Settings.Default.LastItemFetch = value;
    }

    public DateTime LastStyleFetch {
        get => Settings.Default.LastItemFetch;
        set => Settings.Default.LastItemFetch = value;
    }

    public void Save() => Settings.Default.Save();
    public void Reset() => Settings.Default.Reset();
}