using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;
using Attribute = YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects.Attribute;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers;

public class QueryProfileAthenaResponseMapper : IResponseMapper<QueryProfileAthenaResponse> {
    public QueryProfileAthenaResponse? MapFromJson(JToken json) {
        var response = new QueryProfileAthenaResponse {
            Created = json.SelectToken("$.profileChanges[0].profile.created")!.ToObject<DateTime>(),
            Updated = json.SelectToken("$.profileChanges[0].profile.updated")!.ToObject<DateTime>(),
            AccountLevel = json.SelectToken("$.profileChanges[0].profile.stats.attributes.accountLevel")?.ToObject<int>() ?? 0,
            LifetimeWins = json.SelectToken("$.profileChanges[0].profile.stats.attributes.lifetime_wins")?.ToObject<int>() ?? 0,
            BattlePassStars = json.SelectToken("$.profileChanges[0].profile.stats.attributes.battlestars")?.ToObject<int>() ?? 0,
            BattlePassStarsTotal = json.SelectToken("$.profileChanges[0].profile.stats.attributes.battlestars_season_total")?.ToObject<int>() ?? 0
            /*CurrentSeasonStats = new SeasonStats {
                SeasonNumber = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season_num")!.ToObject<int>(),
                Wins = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.numWins")?.ToObject<int>(),
                NumLowBracket = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.numHighBracket")?.ToObject<int>(),
                NumHighBracket = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.numLowBracket")?.ToObject<int>(),

                SeasonXp= json.SelectToken("x.profileChanges[0].profile.stats.attributes.xp")?.ToObject<int>(),
                SeasonLevel = json.SelectToken("x.profileChanges[0].profile.stats.attributes.level")?.ToObject<int>(),
                BattlePassLevel = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.book_level")?.ToObject<int>(),

                PurchasedBattlePass = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.book_level")?.ToObject<int>(),
                CrownedWins = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.royal_royales")?.ToObject<int>(), // TODO: Not sure about this one, check it!!!
                SurvivorTier = json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.survivor_tier")?.ToObject<int>(), 
                SurvivorPrestige= json.SelectToken("x.profileChanges[0].profile.stats.attributes.season.survivor_prestige")?.ToObject<int>()
            }*/
        };

        var itemsTokens = json.SelectTokens("$.profileChanges[0].profile.items.*");
        var items = itemsTokens.Select(ParseItem);
        response.Items = items.ToList();
        return response;
    }

    private Item ParseItem(JToken token) {
        var item = new Item();
        item.FortniteId = token.SelectToken("$.templateId")!.ToObject<string>()!;
        item.Quantity = token.SelectToken("$.quantity")?.ToObject<int>()!;
        item.Attributes = new Attribute();

        var variantTokens = token.SelectToken("$.attributes.variants.*");
        if(variantTokens != null) {
            item.Attributes.Variants = variantTokens.Select(v => v.ToObject<Variant>()!).ToList();
        }
        item.Attributes.GrantAccessToItem = token.SelectToken("$.attributes.access_item")?.ToObject<string>();

        return item;
    }
}