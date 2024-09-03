namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

/// <summary>
/// Represents item obtained return by QueryProfile by EG API
/// </summary>
public class OwnedFortniteItem(string fortniteId, List<OwnedVariant> ownedVariants)
{
    public string FortniteId { get; } = fortniteId;
    public List<OwnedVariant> OwnedStylesRaw { get;} = ownedVariants ?? [];
}

public class OwnedVariant(string channel, List<string> properties) {
    public string Channel { get; }
    public List<string> Properties { get; }
}