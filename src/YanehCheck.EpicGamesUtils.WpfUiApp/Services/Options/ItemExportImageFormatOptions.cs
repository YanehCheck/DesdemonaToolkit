namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemExportImageFormatOptions {
    public const string Key = "Item:Export:Image:Format";

    public string ImageFormat { get; set; } = "JPEG";
    public int ImageJpegQuality { get; set; } = 100;
}