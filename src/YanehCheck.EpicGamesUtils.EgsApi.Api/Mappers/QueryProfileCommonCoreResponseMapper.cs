using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;
using Attribute = YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects.Attribute;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers;

public class QueryProfileCommonCoreResponseMapper : IResponseMapper<QueryProfileCommonCoreResponse> {
    public QueryProfileCommonCoreResponse? MapFromJson(JToken json) {
        var response = new QueryProfileCommonCoreResponse {
            Created = json.SelectToken("$.profileChanges[0].profile.created")!.ToObject<DateTime>(),
            Updated = json.SelectToken("$.profileChanges[0].profile.updated")!.ToObject<DateTime>()
        };

        var itemsTokens = json.SelectTokens("$.profileChanges[0].profile.items.*");
        var items = itemsTokens.Select(t => new Item {
            FortniteId = t.SelectToken("$.templateId")!.ToObject<string>()!,
            Quantity = t.SelectToken("$.quantity")?.ToObject<int>(),
            Attributes = new Attribute()
        });
        response.Items = items.ToList();
        return response;
    }
}