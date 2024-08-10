namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;

public interface ICached {
    public void InvalidateCache();
    public void Invalidate(string method);
}