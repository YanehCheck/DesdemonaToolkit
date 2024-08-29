using System.IO;
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
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;

public class DialogService(IWritableOptions<ItemExportImageFormatOptions> imageSaveOptions) : IDialogService
{
    public async Task SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*")
    {
        var dialog = new SaveFileDialog
        {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if (dialog.ShowDialog() == true)
        {
            string filePath = dialog.FileName;
            await File.WriteAllTextAsync(filePath, content);
        }
    }

    public async Task SaveImageFile(Image image, string fileName, string defaultExt = null!, string filter = null!)
    {
        if (defaultExt is null)
        {
            defaultExt = $".{Enum.GetName(imageSaveOptions.Value.ImageFormat)!.ToLower()}";
            filter = $"Image files (*{defaultExt})|*{defaultExt}|All files (*.*)|*.*";
        }

        var dialog = new SaveFileDialog
        {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if (dialog.ShowDialog() == true)
        {
            var filePath = dialog.FileName;
            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.SaveAsync(stream, GetEncoder());
        }
    }

    public string? OpenFolder() {
        var dialog = new OpenFolderDialog
        {
            Multiselect = false,
            ValidateNames = true
        };

        if (dialog.ShowDialog() == true) {
            return dialog.FolderName;
        }

        return null;
    }

    public ImageEncoder GetEncoder()
    {
        return imageSaveOptions.Value.ImageFormat switch
        {
            ImageFormat.Jpg => new JpegEncoder
            {
                Quality = imageSaveOptions.Value.ImageJpegQuality
            },
            ImageFormat.Png => new PngEncoder(),
            ImageFormat.Bmp => new BmpEncoder(),
            ImageFormat.Gif => new GifEncoder(),
            ImageFormat.Pbm => new PbmEncoder(),
            ImageFormat.Qoi => new QoiEncoder(),
            ImageFormat.Webp => new WebpEncoder(),
            ImageFormat.Tga => new TgaEncoder(),
            ImageFormat.Tiff => new TiffEncoder(),
            _ => new JpegEncoder()
        };
    }
}