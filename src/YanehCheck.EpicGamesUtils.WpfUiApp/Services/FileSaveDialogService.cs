using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Qoi;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class FileSaveDialogService(IOptions<ItemExportImageFormatOptions> imageSaveOptions) : IFileSaveDialogService {
    public async Task SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*") {
        var dialog = new SaveFileDialog {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if(dialog.ShowDialog() == true) {
            string filePath = dialog.FileName;
            await File.WriteAllTextAsync(filePath, content);
        }
    }

    public async Task SaveImageFile(Image image, string fileName, string defaultExt = null!, string filter = null!) {
        if(defaultExt is null) {
            defaultExt = $".{imageSaveOptions.Value.ImageFormat.ToLower()}";
            filter = $"Image files (*{defaultExt})|*{defaultExt}|All files (*.*)|*.*";
        }

        var dialog = new SaveFileDialog {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if(dialog.ShowDialog() == true) {
            var filePath = dialog.FileName;
            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.SaveAsync(stream, GetEncoder());
        }
    }

    public ImageEncoder GetEncoder() {
        return imageSaveOptions.Value.ImageFormat.ToUpper() switch {
            "JPG" or "JPEG" => new JpegEncoder {
                Quality = imageSaveOptions.Value.ImageJpegQuality
            },
            "PNG" => new PngEncoder(),
            "BMP" => new BmpEncoder(),
            "GIF" => new GifEncoder(),
            "PBM" => new PbmEncoder(),
            "QOI" => new QoiEncoder(),
            "WEBP" => new WebpEncoder(),
            "TGA" => new TgaEncoder(),
            "TIFF" => new TiffEncoder(),
            _ => new JpegEncoder()
        };
    }
}
