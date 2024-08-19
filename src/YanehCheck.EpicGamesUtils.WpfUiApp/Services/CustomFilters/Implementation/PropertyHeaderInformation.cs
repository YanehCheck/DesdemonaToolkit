using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;

public class PropertyHeaderInformation(PropertyHeaderType type, string value) {
    public PropertyHeaderType Type { get; init; } = type;
    public string Value { get; init; } = value;
}