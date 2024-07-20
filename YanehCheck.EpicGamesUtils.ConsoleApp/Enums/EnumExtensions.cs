namespace YanehCheck.EpicGamesUtils.ConsoleApp.Enums;

public static class EnumExtensions
{
    public static FetchItemTarget FromStringToFetchItemTarget(string value) {
        return value switch {
            "fgg" or "fortnitegg" or "fortnite.gg" => FetchItemTarget.FortniteGg,
            "gh" or "github" or "stable" => FetchItemTarget.Github,
            _ => throw new ArgumentException($"Unknown item target: {value}")
        };
    }

    public static ItemOutputFormat FromStringToItemOutputFormat(string value) {
        return value switch {
            "raw" => ItemOutputFormat.Raw,
            "list" => ItemOutputFormat.ListOfNames,
            "json" => ItemOutputFormat.Json,
            "web" => ItemOutputFormat.Web,
            _ => throw new ArgumentException($"Unknown output type: {value}")
        };
    }
}