using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api;

public interface ICachedEpicGamesClient : IEpicGamesClient {
    Task PrecacheAsync(Func<Task<IResponse>> methodCall, string methodName);
}