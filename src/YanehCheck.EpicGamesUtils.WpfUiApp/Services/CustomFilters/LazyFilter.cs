using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

/// <summary>
/// Filter, which gets compiled only on its first use.
/// </summary>
/// <param name="parser"></param>
/// <param name="sourceCode"></param>
public class LazyFilter(ICustomFilterParser parser, string sourceCode) : Filter {
    public override IEnumerable<ItemModel> Apply(IEnumerable<ItemModel> items) {
        var compiledFilter = parser.Parse(sourceCode);
        MapFrom(compiledFilter);
        return base.Apply(items);
    }

    private void MapFrom(IFilter filter) {
        Version = filter.Version;
        Name = filter.Name;
        Description = filter.Description;
        Author = filter.Author;
        DnfExpression = filter.DnfExpression;
    }
}