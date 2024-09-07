using YanehCheck.EpicGamesUtils.EgsApi.Service.Dtos;

namespace YanehCheck.EpicGamesUtils.EgsApi.Service;

public interface IEpicGamesService {
    Task<AccountAuthTokenResult> AuthenticateAccountUsingAuthCode(string authcode);
    Task UseSacCode(string accountId, string accessToken, string sacCode);
    Task<FortniteItemsResult> GetFortniteItems(string accountId, string accessToken);
    Task PrecacheResults(string accountId, string accessToken);
}