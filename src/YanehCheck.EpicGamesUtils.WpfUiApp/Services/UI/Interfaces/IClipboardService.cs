namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;

public interface IClipboardService
{
    event EventHandler<ClipboardService.ClipboardChangedEventArgs>? OnClipboardChanged;
    void Start();
    void Stop();
}