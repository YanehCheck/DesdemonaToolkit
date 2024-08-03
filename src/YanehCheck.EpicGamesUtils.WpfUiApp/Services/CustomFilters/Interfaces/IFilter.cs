using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

/// <summary>
/// Filters items. Produced by <see cref="IFilterParser"/>.
/// </summary>
public interface IFilter
{
    int Version { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Author { get; set; }
    OptimizationLevel OptimizationLevel { get; set; }
    IEnumerable<ItemModel> Apply(IEnumerable<ItemModel> items);
}