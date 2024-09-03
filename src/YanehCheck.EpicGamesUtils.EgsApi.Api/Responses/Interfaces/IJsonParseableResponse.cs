using Newtonsoft.Json.Linq;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

/// <summary>
/// <see cref="IResponse"/> that is parseable by using <see cref="JObject"/>.ToObject()
/// </summary>
public interface IJsonParseableResponse : IResponse;