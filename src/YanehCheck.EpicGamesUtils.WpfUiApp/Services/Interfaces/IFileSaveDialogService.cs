using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface IFileSaveDialogService {
    void SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*");
    void SaveImageFile(Image<Bgra32> image, string fileName, string defaultExt = ".png", string filter = "Image files (*.png)|*.png|All files (*.*)|*.*");
}