using System.Net;
using System.Text.Json;

namespace YanehCheck.EpicGamesUtils.Api.Results;

public record ApiResult(HttpStatusCode StatusCode, JsonDocument? Content) {
    public bool Success => (int) StatusCode is > 199 and < 300;

    public static implicit operator bool(ApiResult result) => result.Success;
    public static implicit operator JsonDocument?(ApiResult result) => result.Content;
}