using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

/// <summary>
/// Filters items. Produced by <see cref="ICustomFilterParser"/>.
/// </summary>
public interface IFilter
{
    public int Version { get; set; } 
    string Name { get; set; }
    string Description { get; set; }
    string Author { get; set; }
    public IEnumerable<ChainedCondition> DnfExpression { get; set; }
    IEnumerable<ItemModel> Apply(IEnumerable<ItemModel> items);
}