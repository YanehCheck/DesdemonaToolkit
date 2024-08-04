using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

public class CustomFilterProvider(ICustomFilterParser parser) : ICustomFilterProvider {
    public string FilterDirectory { get; } = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!, "data/filters");

    /// <inheritdoc cref="GetAllAsync"/>
    public IEnumerable<IFilter> GetAll() => GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    
    /// <summary>
    /// Reads, compiles and returns all filters.
    /// </summary>
    /// <exception cref="FilterParserException"></exception>
    public async Task<IEnumerable<IFilter>> GetAllAsync() {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new List<IFilter>();
        foreach(var filter in filterFiles) {
            var sourceCode = await File.ReadAllTextAsync(filter);
            filters.Add(parser.Parse(sourceCode));
        }

        return filters;
    }

    /// <inheritdoc cref="GetAllParallelAsync"/>
    public IEnumerable<IFilter> GetAllParallel() => GetAllParallelAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Reads, compiles and returns all filters in parallel.
    /// </summary>
    /// <exception cref="FilterParserException"></exception>
    public async Task<IEnumerable<IFilter>> GetAllParallelAsync() {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new ConcurrentBag<IFilter>();
        await Parallel.ForEachAsync(filterFiles, async (f, _) => {
            var sourceCode = await File.ReadAllTextAsync(f, _);
            filters.Add(new LazyFilter(parser, sourceCode));
        });

        return filters;
    }

    /// <inheritdoc cref="GetAllLazyAsync"/>
    public IEnumerable<IFilter> GetAllLazy() => GetAllLazyAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Filters get compiled right before their first usage
    /// </summary>
    public async Task<IEnumerable<IFilter>> GetAllLazyAsync() {
        Directory.CreateDirectory(FilterDirectory);
        var filterFiles = Directory.GetFiles(FilterDirectory, "*.dtkf");
        var filters = new List<IFilter>();
        foreach (var filter in filterFiles) {
            var sourceCode = await File.ReadAllTextAsync(filter);
            filters.Add(new LazyFilter(parser, sourceCode));
        }

        return filters;
    }
}