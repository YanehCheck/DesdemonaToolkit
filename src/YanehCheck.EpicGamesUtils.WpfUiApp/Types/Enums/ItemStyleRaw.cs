namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

public record ItemStyleRaw(string channel, IEnumerable<string> owned)
{
    public string Channel { get; set; } = channel;
    public List<string> Property { get; set; } = owned?.ToList() ?? [];
}