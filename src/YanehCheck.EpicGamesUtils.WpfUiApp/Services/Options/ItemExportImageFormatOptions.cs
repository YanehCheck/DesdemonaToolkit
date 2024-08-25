using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemExportImageFormatOptions {
    public const string Key = "Item:Export:Image:Format";

    public ImageFormat ImageFormat { get; set; } = ImageFormat.Jpg;
    public int ImageJpegQuality { get; set; } = 100;
}