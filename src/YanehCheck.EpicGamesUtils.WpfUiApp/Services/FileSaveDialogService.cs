using System.IO;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class FileSaveDialogService : IFileSaveDialogService {
    public void SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*") {
        var dialog = new SaveFileDialog {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if(dialog.ShowDialog() == true) {
            string filePath = dialog.FileName;
            File.WriteAllText(filePath, content);
        }
    }

    public void SaveImageFile(Image<Rgba32> image, string fileName, string defaultExt = ".png", string filter = "Image files (*.png)|*.png|All files (*.*)|*.*") {
        var dialog = new SaveFileDialog {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if(dialog.ShowDialog() == true) {
            string filePath = dialog.FileName;
            using(var stream = new FileStream(filePath, FileMode.Create)) {
                image.Save(stream, new PngEncoder());
            }
        }
    }
}
