using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class SessionService(IPersistenceProvider persistenceProvider) : ISessionService {
    public bool IsAuthenticated =>
        AccountId is not null &&
        AccessToken is not null &&
        AccessTokenExpiry is not null &&
        AccessTokenExpiry > DateTime.Now;
    public string? AccountId { get; set; } = persistenceProvider.AccessTokenExpiry > DateTime.Now ? persistenceProvider.AccountId : null;
    public string? AccessToken { get; set; } = persistenceProvider.AccessTokenExpiry > DateTime.Now ? persistenceProvider.AccessToken : null;
    public DateTime? AccessTokenExpiry { get; set; } = persistenceProvider.AccessTokenExpiry > DateTime.Now ? persistenceProvider.AccessTokenExpiry : null;
    public bool IsItemDataFetched { get; set; } = persistenceProvider.LastItemFetch != DateTime.MinValue;
    public string? DisplayName { get; set; } = persistenceProvider.DisplayName;
}