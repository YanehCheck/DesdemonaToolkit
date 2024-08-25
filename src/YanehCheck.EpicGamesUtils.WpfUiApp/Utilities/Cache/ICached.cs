namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Cache;

public interface ICached
{
    public void InvalidateAll();
    public void Invalidate(string method);
}