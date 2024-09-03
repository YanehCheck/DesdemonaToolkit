using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Exceptions;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers;

public class JsonParseableResponseMapper<TResponse> : IResponseMapper<TResponse> where TResponse : class, IJsonParseableResponse {
    public TResponse? MapFromJson(JToken json)
        => json.ToObject<TResponse>();
}

public class AccountAuthCodeAuthResponseMapper : JsonParseableResponseMapper<AccountAuthCodeAuthResponse>;
public class ClientAuthResponseMapper : JsonParseableResponseMapper<ClientAuthResponse>;
public class AccountLookupResponseMapper : JsonParseableResponseMapper<AccountLookupResponse>;
public class ErrorResponseMapper : JsonParseableResponseMapper<EpicGamesApiException>;