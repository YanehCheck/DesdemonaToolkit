namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes.Presentation;

public class StyleNamePresentation(string name, bool usesChannelPropertyForName)
{
    public string Name { get; } = name;
    public bool UsesChannelPropertyForName { get; } = usesChannelPropertyForName;
};