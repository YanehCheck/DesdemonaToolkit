using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

public class CustomFilterProvider(ICustomFilterParser parser) : ICustomFilterProvider {
    public string FilterDirectory { get; } = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!, "data/filters");

    /// <inheritdoc cref="GetAllAsync"/>
    public IEnumerable<IFilter> GetAll(Action<string, Exception>? onErrorCallback = null) =>
        GetAllAsync(onErrorCallback).ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Reads, compiles and returns all filters.
    /// </summary>
    public async Task<IEnumerable<IFilter>> GetAllAsync(Action<string, Exception>? onErrorCallback = null) {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new List<IFilter>();

        foreach(var filter in filterFiles) {
            try {
                var sourceCode = await File.ReadAllTextAsync(filter);
                filters.Add(parser.Parse(sourceCode));
            }
            catch(Exception ex) {
                // Call the callback with the filter file name and the exception
                onErrorCallback?.Invoke(filter, ex);
            }
        }

        return filters;
    }

    /// <inheritdoc cref="GetAllParallelAsync"/>
    public IEnumerable<IFilter> GetAllParallel(Action<string, Exception>? onErrorCallback = null) =>
        GetAllParallelAsync(onErrorCallback).ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Reads, compiles and returns all filters in parallel.
    /// </summary>
    public async Task<IEnumerable<IFilter>> GetAllParallelAsync(Action<string, Exception>? onErrorCallback = null) {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new ConcurrentBag<IFilter>();

        await Parallel.ForEachAsync(filterFiles, async (filter, _) => {
            try {
                var sourceCode = await File.ReadAllTextAsync(filter, _);
                filters.Add(parser.Parse(sourceCode));
            }
            catch(Exception ex) {
                onErrorCallback?.Invoke(filter, ex);
            }
        });

        return filters;
    }

    /// <inheritdoc cref="GetAllLazyAsync"/>
    public IEnumerable<IFilter> GetAllLazy(Action<string, Exception>? onErrorCallback = null) =>
        GetAllLazyAsync(onErrorCallback).ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Filters get compiled right before their first usage
    /// </summary>
    public async Task<IEnumerable<IFilter>> GetAllLazyAsync(Action<string, Exception>? onErrorCallback = null) {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new List<IFilter>();

        foreach(var filter in filterFiles) {
            try {
                var sourceCode = await File.ReadAllTextAsync(filter);
                filters.Add(new LazyFilter(parser, sourceCode));
            }
            catch(Exception ex) {
                // Call the callback with the filter file name and the exception
                onErrorCallback?.Invoke(filter, ex);
            }
        }

        return filters;
    }
}