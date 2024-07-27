namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

public interface IBrowserService {
    void StartAndNavigate(string url);
    void StartAndNavigateToAuthCode();
}