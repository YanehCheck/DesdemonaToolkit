using System.IO;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class FileSaveDialogService : IFileSaveDialogService {
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

    public async Task SaveImageFile(Image image, string fileName, string defaultExt = ".png", string filter = "Image files (*.png)|*.png|All files (*.*)|*.*") {
        var dialog = new SaveFileDialog {
            FileName = fileName,
            DefaultExt = defaultExt,
            Filter = filter
        };

        if(dialog.ShowDialog() == true) {
            var filePath = dialog.FileName;
            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.SaveAsync(stream, new PngEncoder());
        }
    }
}
