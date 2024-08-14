using SixLabors.ImageSharp;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface IFileSaveDialogService {
    Task SaveTextFile(string content, string fileName, string defaultExt = ".txt", string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*");
    Task SaveImageFile(Image image, string fileName, string defaultExt = null!, string filter = null!);
}