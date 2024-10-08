﻿using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;
using Color = SixLabors.ImageSharp.Color;
using Font = SixLabors.Fonts.Font;
using FontFamily = SixLabors.Fonts.FontFamily;
using Image = SixLabors.ImageSharp.Image;
using ItemPresentationModel = YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes.Presentation.ItemPresentationModel;
using Point = SixLabors.ImageSharp.Point;
using PointF = SixLabors.ImageSharp.PointF;
using Rectangle = SixLabors.ImageSharp.Rectangle;
using RectangleF = SixLabors.ImageSharp.RectangleF;
using Size = SixLabors.ImageSharp.Size;
using TextOptions = SixLabors.Fonts.TextOptions;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteInventoryImageProcessor(IWritableOptions<ItemExportImageAppearanceOptions> options)
    : IFortniteInventoryImageProcessor {
    private readonly int headerHeight = 250;
    private readonly string logoMainText = "DESDEMONA TOOLKIT";
    private readonly string logoSecondaryText = "FREE AND OPEN-SOURCE";
    private readonly string logoTernaryText = "GITHUB.COM/YANEHCHECK/DESDEMONATOOLKIT";
    private readonly int logoSeparatorWidth = 10;

    private readonly ConcurrentDictionary<float, Font> fontCache = new();

    public Image Create(List<ItemPresentationModel> items, string displayName) {
        var itemsPerRow = options.Value.ItemsPerRow;
        var fontFamily = GetFortniteFontFamily();

        var fontColor = Color.ParseHex(options.Value.FontColor);
        var width = itemsPerRow * (options.Value.ItemWidth + options.Value.ItemSpacing) - options.Value.ItemSpacing;
        var height =
            (int) Math.Ceiling((double) items.Count / itemsPerRow) *
            (options.Value.ItemHeight + options.Value.ItemSpacing) - options.Value.ItemSpacing;
        height += options.Value.IncludeHeader ? headerHeight : 0;
        Image image = new Image<Bgra32>(width, height);

        image.Mutate(ctx => ctx.BackgroundColor(Color.ParseHex(options.Value.BackgroundColor)));

        if (options.Value.IncludeHeader) {
            DrawHeaderBase(image, fontColor, fontFamily);
            DrawHeaderStamp(image, fontColor, fontFamily, items.Count, displayName);
        }

        for (int i = 0; i < items.Count; i++) {
            var row = i / itemsPerRow;
            var col = i % itemsPerRow;
            var x = col * (options.Value.ItemWidth + options.Value.ItemSpacing);
            var y = options.Value.IncludeHeader
                ? row * (options.Value.ItemHeight + options.Value.ItemSpacing) + headerHeight
                : row * (options.Value.ItemHeight + options.Value.ItemSpacing);

            using var itemImage = BitmapFrameExtensions.ToImageSharpImage(items[i].BitmapFrame!, out var data);
            var itemName = items[i].Name;
            var itemRemark = items[i].Remark;
            DrawItem(image, itemName, itemRemark, itemImage, x, y, fontColor, fontFamily);
            ArrayPool<byte>.Shared.Return(data);
        }

        return image;
    }

    private void DrawHeaderStamp(Image image, Color fontColor, FontFamily fontFamily, int numberOfItems, string displayName) {
        var spacing = 10;
        var font = fontCache.GetOrAdd(50, fontFamily.CreateFont);
        var textHeight = MeasureTextSize(displayName, font).Height;
        var position = new PointF(20, 20);
        if (options.Value.StampIncludePlayerName) {
            var name = options.Value.StampCensorPlayerName ? CensorDisplayName(displayName.ToUpper()) : displayName.ToUpper();
            image.Mutate(ctx => ctx.DrawText(name, font, fontColor, position));
            position.Y += spacing + textHeight;
        }

        if (options.Value.StampIncludeItemCount) {
            image.Mutate(ctx => ctx.DrawText($"{numberOfItems} ITEMS", font, fontColor, position));
            position.Y += spacing + textHeight;
        }

        if (options.Value.StampIncludeDate) {
            image.Mutate(ctx => ctx.DrawText(DateTime.Now.ToString("dd. MM. yyyy. HH:mm"), font, fontColor, position));
            position.Y += spacing + textHeight;
        }

        if (options.Value.StampIncludeCustomText) {
            image.Mutate(ctx => ctx.DrawText(options.Value.StampCustomText, font, fontColor, position));
            position.Y += spacing + textHeight;
        }
    }

    private void DrawHeaderBase(Image image, Color fontColor, FontFamily fontFamily) {
        // Draw bg
        using var backgroundImage = GetLogoBackground();
        backgroundImage.Mutate(ctx => ctx.Brightness(0.45f));
        image.Mutate(ctx => 
            ctx.DrawImage(backgroundImage, new Point(0, 0), 1)
                .DrawLine(Color.Black, logoSeparatorWidth, [new PointF(0, headerHeight), new PointF(image.Width, headerHeight)]));

        // Draw text
        // Let's not overcomplicate and hard code some of this
        // I could see wanting different logo, but can't ever see someone wanting to specifically change this
        var mainFont = fontCache.GetOrAdd(80, fontFamily.CreateFont);
        var mainSize = MeasureTextSize(logoMainText, mainFont);
        var mainLeftShift = (image.Width - mainSize.Width) / 2;
        var mainTopShift = (headerHeight - mainSize.Height) / 2;
        var mainPosition = new PointF(mainLeftShift, mainTopShift - 30);

        image.Mutate(ctx => ctx.DrawText(logoMainText, mainFont, fontColor, mainPosition));

        var secondaryFont = fontCache.GetOrAdd(50, fontFamily.CreateFont);
        var secondarySize = MeasureTextSize(logoSecondaryText, secondaryFont);
        var secondaryLeftShift = (image.Width - secondarySize.Width) / 2;
        var secondaryTopShift = (headerHeight - secondarySize.Height) / 2;
        var secondaryPosition = new PointF(secondaryLeftShift, secondaryTopShift + 50);
        image.Mutate(ctx => ctx.DrawText(logoSecondaryText, secondaryFont, fontColor, secondaryPosition));

        var ternaryFont = fontCache.GetOrAdd(30, fontFamily.CreateFont);
        var ternarySize = MeasureTextSize(logoTernaryText, ternaryFont);
        var ternaryPosition = new PointF(image.Width - 20 - ternarySize.Width, 0 + 20);
        image.Mutate(ctx => ctx.DrawText(logoTernaryText, ternaryFont, fontColor, ternaryPosition));
    }

    private void DrawItem(Image inventoryImage, string itemName, string? itemRemark, Image itemImage, int x, int y, Color fontColor, FontFamily fontFamily) {
        var itemWidth = options.Value.ItemWidth;
        var itemHeight = options.Value.ItemHeight;
        var nameRectangleHeight = options.Value.NameRectangleHeight;
        var itemNameTransformed = itemName.RemoveAccents().ToUpper();

        // Draw item
        itemImage.Mutate(ctx => ctx.Resize(itemWidth, itemHeight));
        inventoryImage.Mutate(ctx => ctx.DrawImage(itemImage, new Point(x, y), 1));

        // Draw name rectangle and text
        var font = GetFontWithRightSize(itemNameTransformed, fontFamily,
            options.Value.NameFontSize,
            options.Value.NameFontDownsizeStep,
            options.Value.NameTextPaddingLr,
            options.Value.NameTextPaddingTb,
            out var size);
        var leftShift = (itemWidth - size.Width) / 2;
        var topShift = (nameRectangleHeight - size.Height) / 2;
        var nameRect = new RectangleF(x, y + itemHeight - nameRectangleHeight, itemWidth, nameRectangleHeight);
        var nameRectCol = Color.ParseHex(options.Value.NameRectangleColor);
        inventoryImage.Mutate(ctx => ctx.Fill(nameRectCol, nameRect));
        var namePosition = new PointF(x + leftShift, y + itemHeight - nameRectangleHeight + topShift);
        inventoryImage.Mutate(ctx => ctx.DrawText(itemNameTransformed, font, fontColor, namePosition));

        if(itemRemark is not null && options.Value.ItemIncludeRemark) {
            // Draw remark
            var fontRemark = GetFontWithRightSize(itemRemark, fontFamily,
                options.Value.RemarkFontSize,
                options.Value.RemarkFontDownsizeStep,
                options.Value.RemarkTextPaddingLr,
                options.Value.RemarkTextPaddingTb,
                out var remarkSize);

            // TODO: Replace with enum
            var leftShiftRemark = options.Value.RemarkTextAlign switch {
                'L' or 'l' => options.Value.RemarkTextPaddingLr,
                'C' or 'c' => (itemWidth - remarkSize.Width) / 2,
                'R' or 'r' => itemWidth - remarkSize.Width - options.Value.RemarkTextPaddingLr,
                _ => options.Value.RemarkTextPaddingLr
            };

            var remarkBoxHeight = options.Value.RemarkRectangleHeight;
            var remarkBoxWidth = itemWidth;

            var remarkPolygon = new PointF[] {
                new(x, y),
                new(x + remarkBoxWidth, y),
                new(x + remarkBoxWidth, y + remarkBoxHeight),
                new(x, y + remarkBoxHeight + 10)
            };

            var remarkRectColor = Color.ParseHex(options.Value.RemarkRectangleColor);

            var topShiftRemark = (remarkBoxHeight - remarkSize.Height) / 2;

            inventoryImage.Mutate(ctx =>
            {
                
                ctx.FillPolygon(remarkRectColor, remarkPolygon);

                ctx.SetDrawingTransform(Matrix3x2.CreateRotation(-3 * (float) Math.PI / 180, new Vector2(x + remarkBoxWidth / 2, y + remarkBoxHeight / 2)));
                ctx.DrawText(itemRemark, fontRemark, fontColor, new PointF(x + leftShiftRemark, y + topShiftRemark));
            });
        }
    }

    private Font GetFontWithRightSize(string text, FontFamily fontFamily, float initialSize, float sizeDownStep, float paddingLr, float paddingTb, out FontRectangle size) {
        var currentFontSize = initialSize;

        do {
            var font = fontCache.GetOrAdd(currentFontSize, fontFamily.CreateFont);
            size = MeasureTextSize(text, font);
            if (size.Width <= options.Value.ItemWidth - paddingLr * 2 &&
                size.Height <= options.Value.NameRectangleHeight - paddingTb * 2) {
                return font;
            }

            currentFontSize -= sizeDownStep;
        }
        while (true);
    }

    private FontRectangle MeasureTextSize(string text, Font font) {
        return TextMeasurer.MeasureSize(text, new TextOptions(font));
    }

    private FontFamily GetFortniteFontFamily() {
        var uri = new Uri("pack://application:,,,/Assets/Fonts/Fortnite.ttf");
        using var resourceStream = Application.GetResourceStream(uri)?.Stream;

        using var memoryStream = new MemoryStream();
        resourceStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        FontCollection fontCollection = new FontCollection();
        return fontCollection.Add(memoryStream);
    }

    private Image GetLogoBackground() {
        var uri = new Uri("pack://application:,,,/Assets/Images/desdemona-bg-1920.jpg");
        using var resourceStream = Application.GetResourceStream(uri)?.Stream;

        using var memoryStream = new MemoryStream();
        resourceStream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        var image = Image.Load(memoryStream);

        // Scale and crop
        var oldWidth = image.Width;
        var newWidth = options.Value.ItemsPerRow * (options.Value.ItemWidth + options.Value.ItemSpacing);
        image.Mutate(ctx => ctx.Resize(new Size(newWidth, 0)));
        var cropY = (int) ((double) image.Width / oldWidth * 350);
        image.Mutate(ctx => ctx.Crop(new Rectangle(0, cropY, newWidth, headerHeight)));
        return image;
    }

    /// <summary>
    /// The censoring is dynamic to character length
    /// </summary>
    private string CensorDisplayName(string name) {
        return name.Length switch {
            <= 4 => name[0] + new string('*', name.Length - 1),
            <= 6 => name[..2] + new string('*', name.Length - 2),
            _ => name[..2] + new string('*', name.Length - 4) + name[^2..]
        };
    }
}