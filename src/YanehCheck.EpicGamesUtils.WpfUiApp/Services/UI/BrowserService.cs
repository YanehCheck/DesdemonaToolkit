﻿using System.Diagnostics;
using System.IO;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;

public class BrowserService : IBrowserService
{
    public void StartAndNavigate(string url)
    {
        Process.Start("explorer", $"\"{url}\"");
    }

    public void StartAndNavigateToAuthCodeWebsite()
    {
        Process.Start("explorer", "\"https://epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code\"");
    }

    public void StartAndNavigateToDataDirectory()
    {
        Process.Start("explorer", Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data"));
    }

    public void StartAndNavigateToAppSettings() {
        Process.Start("explorer", Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "appsettings.json"));
    }
}