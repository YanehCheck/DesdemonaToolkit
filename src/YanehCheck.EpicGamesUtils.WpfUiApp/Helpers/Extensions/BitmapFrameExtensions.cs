using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Extensions;

public static class BitmapFrameExtensions {
    public static Image<Bgra32> ToImageSharpImage(this BitmapFrame bitmapFrame) {

        if(bitmapFrame == null) {
            throw new ArgumentNullException(nameof(bitmapFrame));
        }

        var width = bitmapFrame.PixelWidth;
        var height = bitmapFrame.PixelHeight;
        var stride = width * 4; 

        var pixelData = new byte[height * stride];

        bitmapFrame.CopyPixels(pixelData, stride, 0);
        var image = Image.LoadPixelData<Bgra32>(pixelData, width, height);

        return image;
    }
}