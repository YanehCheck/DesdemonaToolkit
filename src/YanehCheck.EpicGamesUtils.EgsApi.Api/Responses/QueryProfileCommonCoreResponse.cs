using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;

/// <summary>
/// <remarks>Structure doesn't correspond to the JSON and doesn't include all the information!</remarks>
/// </summary>
public class QueryProfileCommonCoreResponse : IResponse {
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public List<Item> Items { get; set; } = [];

}