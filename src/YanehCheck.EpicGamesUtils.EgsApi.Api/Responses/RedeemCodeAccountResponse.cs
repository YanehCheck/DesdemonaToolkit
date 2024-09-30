using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;

public class RedeemCodeAccountResponse : IJsonParseableResponse {
    [JsonProperty("offerId")]
    public string OfferId { get; set; }

    [JsonProperty("accountId")]
    public string AccountId { get; set; }

    [JsonProperty("identityId")]
    public string IdentityId { get; set; }
}

