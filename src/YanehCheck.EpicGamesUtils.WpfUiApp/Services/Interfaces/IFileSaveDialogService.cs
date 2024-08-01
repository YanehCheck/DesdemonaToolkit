using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface IFileSaveDialogService {
    Task SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*");
    Task SaveImageFile(Image<Bgra32> image, string fileName, string defaultExt = ".png", string filter = "Image files (*.png)|*.png|All files (*.*)|*.*");
}