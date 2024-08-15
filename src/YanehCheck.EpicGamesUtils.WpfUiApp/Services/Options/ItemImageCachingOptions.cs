namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemImageCachingOptions {
    public const string Key = "Item:Image:Caching";

    public bool CacheDownloadedImages { get; set; } = true;
}