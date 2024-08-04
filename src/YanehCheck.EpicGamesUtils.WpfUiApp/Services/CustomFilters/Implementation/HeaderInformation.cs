using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;

public class HeaderInformation(HeaderType type, string value) {
    public HeaderType Type { get; init; } = type;
    public string Value { get; init; } = value;
}