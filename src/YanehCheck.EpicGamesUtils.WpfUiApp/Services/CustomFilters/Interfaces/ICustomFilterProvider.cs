using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

public interface ICustomFilterProvider {
    string FilterDirectory { get; }

    /// <inheritdoc cref="GetAllAsync"/>
    IEnumerable<IFilter> GetAll();

    /// <summary>
    /// Reads, compiles and returns all filters.
    /// </summary>
    /// <exception cref="FilterParserException"></exception>
    Task<IEnumerable<IFilter>> GetAllAsync();

    /// <inheritdoc cref=".GetAllParallelAsync"/>
    IEnumerable<IFilter> GetAllParallel();

    /// <summary>
    /// Reads, compiles and returns all filters in parallel.
    /// </summary>
    /// <exception cref="FilterParserException"></exception>
    Task<IEnumerable<IFilter>> GetAllParallelAsync();

    /// <inheritdoc cref="GetAllLazyAsync"/>
    IEnumerable<IFilter> GetAllLazy();

    /// <summary>
    /// Filters get compiled right before their first usage
    /// </summary>
    Task<IEnumerable<IFilter>> GetAllLazyAsync();
}