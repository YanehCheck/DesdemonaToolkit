using Microsoft.Extensions.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
{
    void Update(Action<T> applyChanges);
}