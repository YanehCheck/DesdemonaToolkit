namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;

public interface IBrowserService {
    void StartAndNavigate(string url);
    void StartAndNavigateToAuthCodeWebsite(); 
    void StartAndNavigateToDataDirectory();
}