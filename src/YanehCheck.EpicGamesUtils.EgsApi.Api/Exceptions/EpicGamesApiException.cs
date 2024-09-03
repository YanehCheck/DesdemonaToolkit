using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Exceptions;

[method: JsonConstructor]
public class EpicGamesApiException(
    [JsonProperty("errorCode")] string errorCode,
    [JsonProperty("errorMessage")] string errorMessage,
    [JsonProperty("numericErrorCode")] int numericErrorCode,
    [JsonProperty("originatingService")] string originatingService,
    [JsonProperty("intent")] string intent,
    [JsonProperty("error_description")] string? errorDescription = null,
    [JsonProperty("error")] string? error = null)
    : Exception(errorMessage), IJsonParseableResponse {
    [JsonProperty("errorCode")]
    public string ErrorCode { get; } = errorCode;

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; } = errorMessage;

    [JsonProperty("numericErrorCode")]
    public int NumericErrorCode { get; } = numericErrorCode;

    [JsonProperty("originatingService")]
    public string OriginatingService { get; } = originatingService;

    [JsonProperty("intent")]
    public string Intent { get; } = intent;

    [JsonProperty("error_description")]
    public string? ErrorDescription { get; } = errorDescription;

    [JsonProperty("error")]
    public string? Error { get; } = error;
}
