namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

public interface IOptionsWriter<T>
{
    public void Update(Action<T> callback, bool reload = true);
}