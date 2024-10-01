using WK.Libraries.SharpClipboardNS;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;

public class ClipboardService : IClipboardService, IDisposable {
    private readonly SharpClipboard clipboard = new();

    public event EventHandler<ClipboardChangedEventArgs>? OnClipboardChanged;

    public ClipboardService() {
        clipboard.ObserveLastEntry = false;
        clipboard.MonitorClipboard = false;
        clipboard.ClipboardChanged += Clipboard_ClipboardChanged!;
        clipboard.StartMonitoring();
    }

    private void Clipboard_ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e) {
        var convertedContent = ConvertContent(e.Content);
        var convertedType = ConvertContentType(e.ContentType);
        OnClipboardChanged?.Invoke(this, new ClipboardChangedEventArgs(convertedContent, convertedType));
    }

    public void Start() {
        clipboard.MonitorClipboard = true;
    }

    public void Stop() {
        clipboard.MonitorClipboard = false;
    }

    private object ConvertContent(object content) => content;

    private ContentType ConvertContentType(SharpClipboard.ContentTypes contentType) {
        return contentType switch {
            SharpClipboard.ContentTypes.Text => ContentType.Text,
            SharpClipboard.ContentTypes.Files => ContentType.Files,
            _ => ContentType.Other
        };
    }

    public void Dispose() {
        clipboard.StopMonitoring();
    }

    public record ClipboardChangedEventArgs(object Content, ContentType Type);

    public enum ContentType {
        Text,
        Files,
        Other
    }
}