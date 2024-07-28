using System.Net;
using Newtonsoft.Json.Linq;

namespace YanehCheck.EpicGamesUtils.Api.Results;

public record ApiResult(HttpStatusCode StatusCode, JObject? Content) {
    public bool Success => (int) StatusCode is > 199 and < 300;

    public static implicit operator bool(ApiResult result) => result.Success;
    public static implicit operator JObject?(ApiResult result) => result.Content;
}