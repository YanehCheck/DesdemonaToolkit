using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

///<inheritdoc cref="IFilter"/>
public class Filter : IFilter {
    public int Version { get; set; } = 1;
    public string Name { get; set; } = "Untitled filter";
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = "Unknown";
    // Will be used later
    public OptimizationLevel OptimizationLevel { get; set; } = OptimizationLevel.L0;
    public IEnumerable<ChainedCondition> DnfExpression { get; set; } = [];
    public ChainedCondition? LastClause => DnfExpression.LastOrDefault()?.Last();
    public void AddClause(bool conjunction = false) {
        if (!DnfExpression.Any()) {
            DnfExpression = DnfExpression.Append(new ChainedCondition());
            return;
        }

        if (conjunction == true) {
            LastClause!.FollowingTerm = new ChainedCondition();
        }
        else {
            DnfExpression = DnfExpression.Append(new ChainedCondition());
        }
    }

    public virtual IEnumerable<ItemModel> Apply(IEnumerable<ItemModel> items) {
        return !DnfExpression.Any() ? 
            items : 
            items.Where(i => DnfExpression.Any(r => r.Satisfied(i)));
    }
}