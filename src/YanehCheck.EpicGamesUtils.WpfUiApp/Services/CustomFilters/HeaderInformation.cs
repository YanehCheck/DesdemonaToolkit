using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

public class HeaderInformation(HeaderType type, string value) {
    public HeaderType Type { get; init; } = type;
    public string Value { get; init; } = value;
}