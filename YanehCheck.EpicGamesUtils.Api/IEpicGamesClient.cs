using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Stw;

namespace YanehCheck.EpicGamesUtils.Api;

public interface IEpicGamesClient {
    public Task<ApiResult> AuthenticateAsClient(AuthClientType clientType);
    public Task<ApiResult> AuthenticateAsAccount(AuthClientType clientType, string authCode);
    public Task<ApiResult> GetFounderCodes(FounderCodePlatform platform);
    public Task<ApiResult> GetInventory();
}