using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

public interface ICustomFilterProvider {
    string FilterDirectory { get; }

    /// <inheritdoc cref="GetAllAsync"/>
    IEnumerable<IFilter> GetAll(Action<string, Exception>? onErrorCallback = null);

    /// <summary>
    /// Reads, compiles and returns all filters.
    /// </summary>
    /// <param name="onErrorCallback">Gets called on exception with the file name and Exception.</param>
    /// <exception cref="FilterParserException"></exception>
    Task<IEnumerable<IFilter>> GetAllAsync(Action<string, Exception>? onErrorCallback = null);

    /// <inheritdoc cref=".GetAllParallelAsync"/>
    IEnumerable<IFilter> GetAllParallel(Action<string, Exception>? onErrorCallback = null);

    /// <summary>
    /// Reads, compiles and returns all filters in parallel.
    /// </summary>
    /// <param name="onErrorCallback">Gets called on exception with the file name and Exception.</param>
    /// <exception cref="FilterParserException"></exception>
    Task<IEnumerable<IFilter>> GetAllParallelAsync(Action<string, Exception>? onErrorCallback = null);

    /// <inheritdoc cref="GetAllLazyAsync"/>
    IEnumerable<IFilter> GetAllLazy(Action<string, Exception>? onErrorCallback = null);

    /// <summary>
    /// Filters get compiled right before their first usage
    /// <param name="onErrorCallback">Gets called on exception with the file name and Exception.</param>
    /// </summary>
    Task<IEnumerable<IFilter>> GetAllLazyAsync(Action<string, Exception>? onErrorCallback = null);
}