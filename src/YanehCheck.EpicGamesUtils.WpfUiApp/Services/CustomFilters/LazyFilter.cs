using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

/// <summary>
/// Filter, which gets compiled only on its first use.
/// </summary>
/// <param name="parser"></param>
/// <param name="sourceCode"></param>
public class LazyFilter(ICustomFilterParser parser, string sourceCode) : Filter {
    private bool parsed = false;
    public override IEnumerable<ItemOwnedModel> Apply(IEnumerable<ItemOwnedModel> items) {
        if (!parsed) {
            var parsedFilter = parser.Parse(sourceCode);
            MapFrom(parsedFilter);
        }
        return base.Apply(items);
    }

    public override bool Apply(ItemOwnedModel item) {
        if(!parsed) {
            var parsedFilter = parser.Parse(sourceCode);
            MapFrom(parsedFilter);
        }
        return base.Apply(item);
    }

    private void MapFrom(IFilter filter) {
        Version = filter.Version;
        Name = filter.Name;
        Description = filter.Description;
        Author = filter.Author;
        DnfExpression = filter.DnfExpression;
    }
}