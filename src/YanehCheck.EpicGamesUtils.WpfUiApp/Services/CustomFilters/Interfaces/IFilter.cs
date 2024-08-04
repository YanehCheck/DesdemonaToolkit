using YanehCheck.EpicGamesUtils.BL.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

/// <summary>
/// Filters items. Produced by <see cref="IFilterParser"/>.
/// </summary>
public interface IFilter
{
    string Name { get; set; }
    string Description { get; set; }
    string Author { get; set; }
    IEnumerable<ItemModel> Apply(IEnumerable<ItemModel> items);
}