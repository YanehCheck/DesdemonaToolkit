namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemFetchOptions {
    public const string Key = "Item:Fetch";

    public int FortniteGgIdRange { get; set; } = 14000;

    public string StableSourceUri { get; set; } = // TODO: UPDATE THE REMOTE FILENAME
        "https://raw.githubusercontent.com/YanehCheck/fortnite-cosmetic-list/main/items.json";
}