using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Extensions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
using Point = SixLabors.ImageSharp.Point;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public interface IFortniteInventoryImageProcessor {
    Image Create(List<ItemPresentationModel> items);
}
public class FortniteInventoryImageProcessor : IFortniteInventoryImageProcessor {
    private readonly bool addWatermark = true;
    private int watermarkHeight = 250;
    private readonly string watermarkMainText = "DESDEMONA TOOLKIT";
    private readonly string watermarkSecondaryText = "FREE AND OPEN-SOURCE.";
    private readonly string watermarkTernaryText = "GITHUB.COM/YANEHCHECK/DESDEMONATOOLKIT";

    private int itemsPerRow = 10;
    private readonly int spacing = 0;

    private readonly int itemHeight = 260;
    private readonly int itemWidth = 210;
    private readonly Color backgroundColor = Color.Black;

    private readonly int nameTextPaddingLr = 5;
    private readonly int nameTextPaddingTb = 2;
    private readonly byte nameRectangleTransparency = 160;
    private readonly int nameRectangleHeight = 40;
    private readonly Color fontColor = Color.White;
    private readonly float fontSize = 35f;
    private readonly float fontDownsizeStep = 1f;

    public Image Create(List<ItemPresentationModel> items) {
        if (items.Count < 10) {
            itemsPerRow = items.Count;
        }

        int imageHeight = itemHeight;
        var width = itemsPerRow * (itemWidth + spacing) - spacing;
        var height = (int) Math.Ceiling((double) items.Count / itemsPerRow) * (imageHeight + spacing) - spacing;
        height += addWatermark ? watermarkHeight : 0;
        Image image = new Image<Bgra32>(width, height);

        image.Mutate(ctx => ctx.BackgroundColor(backgroundColor));

        if (items.Count > 10 && addWatermark) {
            DrawLogo(image);
        }

        for (int i = 0; i < items.Count; i++) {
            var row = i / itemsPerRow;
            var col = i % itemsPerRow;
            var x = col * (itemWidth + spacing);
            var y = addWatermark ? 
                row * (imageHeight + spacing) + watermarkHeight : 
                row * (imageHeight + spacing);

            using var itemImage = items[i].BitmapFrame!.ToImageSharpImage();
            var itemName = items[i].Name;
            DrawItem(image, itemName?.ToUpper() ?? "", itemImage, x, y);
        }
        
        return image;
    }

    private void DrawLogo(Image image) {
        var family = GetFortniteFontFamily();
        var mainFont = family.CreateFont(80);
        var mainSize = MeasureTextSize(watermarkMainText, mainFont);
        var mainLeftShift = (image.Width - mainSize.Width) / 2;
        var mainTopShift = (watermarkHeight - mainSize.Height) / 2;
        var mainPosition = new PointF(0 + mainLeftShift, 0 + mainTopShift - 30);

        image.Mutate(ctx => ctx.DrawText(watermarkMainText, mainFont, fontColor, mainPosition));

        var secondaryFont = family.CreateFont(50);
        var secondarySize = MeasureTextSize(watermarkMainText, secondaryFont);
        var secondaryLeftShift = (image.Width - secondarySize.Width) / 2;
        var secondaryTopShift = (watermarkHeight - secondarySize.Height) / 2;
        var secondaryPosition = new PointF(0 + secondaryLeftShift, secondaryTopShift + 50);
        image.Mutate(ctx => ctx.DrawText(watermarkSecondaryText, secondaryFont, fontColor, secondaryPosition));

        var ternaryFont = family.CreateFont(30);
        var ternarySize = MeasureTextSize(watermarkTernaryText, ternaryFont);
        var ternaryPosition = new PointF(image.Width - 20 - ternarySize.Width, 0 + 20);
        image.Mutate(ctx => ctx.DrawText(watermarkTernaryText, ternaryFont, fontColor, ternaryPosition));
    }

    private void DrawItem(Image inventoryImage, string itemName, Image itemImage, int x, int y) {
        // Draw item
        itemImage.Mutate(ctx => ctx.Resize(itemWidth, itemHeight)); // Just to be sure
        inventoryImage.Mutate(ctx => ctx.DrawImage(itemImage, new Point(x, y), 1));

        var font = GetFontWithRightSize(itemName, out var size);
        var leftShift = (itemWidth - size.Width) / 2;
        var topShift = (nameRectangleHeight - size.Height) / 2;
        // Draw transparent rectangle for name
        var rectangle = new RectangleF(x, y + itemHeight - nameRectangleHeight, itemWidth, nameRectangleHeight);
        var fillColor = new Rgba32(0, 0, 0, nameRectangleTransparency);
        inventoryImage.Mutate(ctx => ctx.Fill(fillColor, rectangle));

        // Draw name
        var namePosition = new PointF(x + leftShift, y + itemHeight - nameRectangleHeight + topShift);
        inventoryImage.Mutate(ctx => ctx.DrawText(itemName, font, fontColor, namePosition));
    }

    private Font GetFontWithRightSize(string text, out FontRectangle size) {
        var fontFamily = GetFortniteFontFamily();
        var currentFontSize = fontSize;

        do {
            var font = fontFamily.CreateFont(currentFontSize);
            size = MeasureTextSize(text, font);
            if(size.Width <= itemWidth - nameTextPaddingLr * 2 && 
               size.Height <= nameRectangleHeight - nameTextPaddingTb * 2) {
                break;
            }

            currentFontSize -= fontDownsizeStep;
        } while(true);

        return fontFamily.CreateFont(currentFontSize);
    }

    private FontRectangle MeasureTextSize(string text, Font font) {
        return TextMeasurer.MeasureSize(text, new TextOptions(font));
    }

    public static FontFamily GetFortniteFontFamily() {
        var uri = new Uri("pack://application:,,,/Assets/Fonts/Fortnite.ttf");
        using var resourceStream = Application.GetResourceStream(uri)?.Stream;

        using var memoryStream = new MemoryStream();
        resourceStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        FontCollection fontCollection = new FontCollection();
        return fontCollection.Add(memoryStream);
    }
}