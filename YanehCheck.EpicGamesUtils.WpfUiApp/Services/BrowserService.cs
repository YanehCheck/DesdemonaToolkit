using System.Diagnostics;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services;

public class BrowserService : IBrowserService {
    public void StartAndNavigate(string url) {
        Process.Start("explorer", $"\"{url}\"");
    }

    public void StartAndNavigateToAuthCode() {
        Process.Start("explorer", "\"https://epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code\"");
    }
}