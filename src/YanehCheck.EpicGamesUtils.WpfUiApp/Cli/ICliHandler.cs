namespace YanehCheck.EpicGamesUtils.WpfUiApp.Cli;

public interface ICliHandler {
    Task HandleAsync(string[] args);
    void ShowConsole();
    void CloseConsole();
}