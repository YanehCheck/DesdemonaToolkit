using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers.Interfaces;

/// <summary>
/// Maps parsed json from the API to <see cref="IResponse"/> object.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
internal interface IResponseMapper<out TResponse> where TResponse : IResponse {
    TResponse? MapFromJson(JToken json);
}