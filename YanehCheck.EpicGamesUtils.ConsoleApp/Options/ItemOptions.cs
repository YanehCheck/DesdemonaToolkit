namespace YanehCheck.EpicGamesUtils.ConsoleApp.Options;

public class ItemOptions {
    public const string Items = "Items";

    public string StableItemDataUri { get; set; } =
        "https://raw.githubusercontent.com/YanehCheck/fortnite-cosmetic-list/main/2024-07-19.json";

    public int DefaultFortniteGgMaxId { get; set; } = 14000;

    public string ItemsSavePath { get; set; } = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "YanehCheck.FortniteUtils/items.json");
}