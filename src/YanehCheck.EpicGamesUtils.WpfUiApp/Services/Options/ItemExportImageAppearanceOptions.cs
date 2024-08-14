 namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

public class ItemExportImageAppearanceOptions {
    public const string Key = "Item:Export:Image:Appearance";
    public bool IncludeHeader { get; set; } = true;
    public bool StampIncludePlayerName { get; set; } = true;
    public bool StampCensorPlayerName { get; set; } = false;
    public bool StampIncludeItemCount { get; set; } = true;
    public bool StampIncludeDate { get; set; } = true;
    public bool StampIncludeCustomText { get; set; } = false;
    public string StampCustomText { get; set; } = "";

    public string BackgroundColor { get; set; } = "#00000000";
    public int ItemHeight { get; set; } = 260;
    public int ItemWidth { get; set; } = 210;
    public int ItemsPerRow { get; set; } = 10;
    public int ItemSpacing { get; set; } = 1;
    public int NameTextPaddingLr { get; set; } = 5;
    public int NameTextPaddingTb { get; set; } = 2;
    public int NameRectangleHeight { get; set; } = 40;
    public byte NameRectangleTransparency { get; set; } = 160;
    public string NameFontColor { get; set; } = "#FFFFFF00";
    public int NameFontSize { get; set; } = 35;
    public int NameFontDownsizeStep { get; set; } = 1;
}