using Newtonsoft.Json;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;

public class Item {
    public string FortniteId { get; set; }
    public Attribute Attributes { get; set; }
    public int? Quantity { get; set; }
}

public class Attribute {
    /// <summary>
    /// Styles
    /// </summary>
    public List<Variant>? Variants { get; set; }
    /// <summary>
    /// Has FortniteId of the item. Used for built-in styles by "ItemAccessToken:xxx" items.
    /// </summary>
    public string? GrantAccessToItem { get; set; }
    
    // And huge number of other attributes which would be a pain in the arse to parse. Guess I can always add more as needed.
}

/// <summary>
/// Represents owned unlockable styles
/// </summary>
public class Variant {
    [JsonProperty("channel")]
    public string Channel { get; set; }
    [JsonProperty("active")]
    public string Active { get; set; }
    [JsonProperty("owned")]
    public List<string> OwnedProperties { get; set; }
}