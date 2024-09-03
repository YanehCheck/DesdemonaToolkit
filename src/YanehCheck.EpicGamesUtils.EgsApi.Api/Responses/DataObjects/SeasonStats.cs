using Newtonsoft.Json;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;

// Currently unused
public class SeasonStats {
    [JsonProperty("seasonNumber")]
    public int SeasonNumber { get; set; }

    [JsonProperty("numWins")]
    public int? Wins { get; set; }

    [JsonProperty("numHighBracket")]
    public int? NumHighBracket { get; set; }

    [JsonProperty("numLowBracket")]
    public int? NumLowBracket { get; set; }

    [JsonProperty("seasonXp")]
    public int? SeasonXp { get; set; }

    [JsonProperty("seasonLevel")]
    public int? SeasonLevel { get; set; }

    [JsonProperty("bookLevel")]
    public int? BattlePassLevel { get; set; }

    [JsonProperty("purchasedVIP")]
    public bool PurchasedBattlePass { get; set; }

    [JsonProperty("numRoyalRoyales")]
    public int? CrownedWins { get; set; }

    [JsonProperty("survivorTier")]
    public int? SurvivorTier { get; set; }

    [JsonProperty("survivorPrestige")]
    public int? SurvivorPrestige { get; set; }
}