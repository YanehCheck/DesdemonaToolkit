namespace YanehCheck.EpicGamesUtils.WpfUiApp.Models;

public record ItemStyleRaw(string channel, IEnumerable<string> owned) {
    public string Channel { get; set; } = channel;
    public List<string> Owned { get; set; } = owned.ToList();
}