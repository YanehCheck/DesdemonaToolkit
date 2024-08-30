using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;

public interface IPersistenceProvider
{
    public string AccountId { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiry { get; set; }
    public FetchStatus ItemFetchStatus { get; set; }
    public FetchStatus StyleFetchStatus { get; set; }
    public DateTime LastItemFetch { get; set; }
    public DateTime LastStyleFetch { get; set; }
    public string DisplayName { get; set; }
    public void Save();
    public void Reset();
}