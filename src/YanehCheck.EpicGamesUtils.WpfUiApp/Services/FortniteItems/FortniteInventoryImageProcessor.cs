using SixLabors.ImageSharp;
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
    // TODO: Un-hardcode this
    private readonly int itemHeight = 260;
    private readonly int itemWidth = 210;
    private readonly Color backgroundColor = Color.Gray;
    private readonly int spacing = 5;

    private int itemsPerRow = 10;

    public Image Create(List<ItemPresentationModel> items) {
        if (items.Count < 10) {
            itemsPerRow = items.Count;
        }

        var width = itemsPerRow * itemWidth;
        var height = (int) Math.Ceiling((double) items.Count / itemsPerRow) * itemHeight;
        var image = new Image<Bgra32>(width, height);

        image.Mutate(ctx => ctx.BackgroundColor(backgroundColor));

        for (int i = 0; i < items.Count; i++) {
            var row = i / itemsPerRow;
            var col = i % itemsPerRow;
            var x = col * (itemWidth + spacing);
            var y = row * (itemHeight + spacing);

            // TODO: Handle this
            using var itemImage = items[i].BitmapFrame!.ToImageSharpImage();
            itemImage.Mutate(ctx => ctx.Resize(itemWidth, itemHeight));
            image.Mutate(ctx => ctx.DrawImage(itemImage, new Point(x, y), 1));
        }

        return image;
    }
}