namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Models;

public class StyleNamePresentation(string name, bool usesChannelPropertyForName) {
    public string Name { get; } = name;
    public bool UsesChannelPropertyForName { get; } = usesChannelPropertyForName;
};