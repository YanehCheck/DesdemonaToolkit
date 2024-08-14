namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;

public interface ICached {
    public void InvalidateAll();
    public void Invalidate(string method);
}