using System.Buffers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities;

public static class BitmapFrameExtensions
{

    /// <summary>
    /// Supports color spaces Bgra32, Bgr32 and Gray8.
    /// <param name="pixelData">Should be freed by ArrayPool{byte} after usage</param>
    /// </summary>
    public static Image ToImageSharpImage(this BitmapFrame bitmapFrame, out byte[] pixelData)
    {
        if (bitmapFrame == null)
        {
            throw new ArgumentNullException("BitmapFrame was null when converting to Image.");
        }

        var width = bitmapFrame.PixelWidth;
        var height = bitmapFrame.PixelHeight;
        var format = bitmapFrame.Format;
        int bytesPerPixel = GetBytesPerPixel(format);
        int stride = width * bytesPerPixel;

        pixelData = ArrayPool<byte>.Shared.Rent(height * stride);
        bitmapFrame.CopyPixels(pixelData, stride, 0);


        Span<byte> pixelSpan = new Span<byte>(pixelData, 0, height * stride);
        if (format == PixelFormats.Bgra32 || format == PixelFormats.Bgr32)
        {
            return Image.LoadPixelData<Bgra32>(pixelSpan, width, height);
        }
        else // format == PixelFormats.Gray8
        {
            return Image.LoadPixelData<L8>(pixelSpan, width, height);
        }

    }

    /// <summary>
    /// Supports color spaces Bgra32, Bgr32 and Gray8. 
    /// </summary>
    public static Image ToImageSharpImage(this BitmapFrame bitmapFrame)
    {
        if (bitmapFrame == null)
        {
            throw new ArgumentNullException("BitmapFrame was null when converting to Image.");
        }

        var width = bitmapFrame.PixelWidth;
        var height = bitmapFrame.PixelHeight;
        var format = bitmapFrame.Format;
        int bytesPerPixel = GetBytesPerPixel(format);
        int stride = width * bytesPerPixel;

        var pixelData = new byte[height * stride];
        bitmapFrame.CopyPixels(pixelData, stride, 0);

        Image image;
        if (format == PixelFormats.Bgra32 || format == PixelFormats.Bgr32)
        {
            image = Image.LoadPixelData<Bgra32>(pixelData, width, height);
        }
        else // format == PixelFormats.Gray8
        {
            image = Image.LoadPixelData<L8>(pixelData, width, height);
        }

        return image;
    }

    private static int GetBytesPerPixel(PixelFormat format)
    {
        int bytesPerPixel;
        if (format == PixelFormats.Bgra32)
        {
            bytesPerPixel = 4;
        }
        else if (format == PixelFormats.Bgr32)
        {
            bytesPerPixel = 4;
        }
        else if (format == PixelFormats.Gray8)
        {
            bytesPerPixel = 1;
        }
        else
        {
            throw new NotSupportedException($"Pixel format {format} is not supported.");
        }

        return bytesPerPixel;
    }
}