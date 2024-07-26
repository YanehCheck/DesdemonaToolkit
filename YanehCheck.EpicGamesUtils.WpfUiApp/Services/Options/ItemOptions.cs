namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemFetchOptions {
    public const string Key = "Items:Fetch";

    public int FortniteGgIdRange { get; set; } = 14000;

    public string StableSourceUri { get; set; } = // TODO: UPDATE THE REMOTE FILENAME
        "https://raw.githubusercontent.com/YanehCheck/fortnite-cosmetic-list/main/2024-07-19.json";
}