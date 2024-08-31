using System.Text.RegularExpressions;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

///<inheritdoc cref="IFilter"/>ter"/>
public class Filter : IFilter {
    public int Version { get; set; } = 1;
    public string Name { get; set; } = "Untitled filter";
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = "Unknown";
    // Will be used later
    public OptimizationLevel OptimizationLevel { get; set; } = OptimizationLevel.L0;
    // Makes every single item satisfy this filter, mainly used for filters that just add remarks
    public bool AllPass { get; set; } = false;
    public IEnumerable<ChainedCondition> DnfExpression { get; set; } = [];
    public ChainedCondition? LastClause => DnfExpression.LastOrDefault()?.Last();
    public void AddClause(bool conjunction = false) {
        if(!DnfExpression.Any()) {
            DnfExpression = DnfExpression.Append(new ChainedCondition());
            return;
        }

        if(conjunction) {
            LastClause!.FollowingTerm = new ChainedCondition();
        }
        else {
            DnfExpression = DnfExpression.Append(new ChainedCondition());
        }
    }

    public virtual IEnumerable<ItemOwnedModel> Apply(IEnumerable<ItemOwnedModel> items) {
        if(!DnfExpression.Any()) {
            return items;
        }

        var filteredItems = items.Where(i => DnfExpression.Any(r => {
            var sat = r.Satisfied(i);
            if(sat) {
                i.Remark = InterpolateVarsInRemark(i, r.Remark);
            }
            return sat;
        }));
        return AllPass ? items : filteredItems;
    }

    public virtual bool Apply(ItemOwnedModel item) {
        if(!DnfExpression.Any()) {
            return true;
        }

        var filterResult = DnfExpression.Any(r => {
            var sat = r.Satisfied(item);
            if(sat) {
                item.Remark = InterpolateVarsInRemark(item, r.Remark);
            }
            return sat;
        });
        return AllPass || filterResult;
    }

    private string? InterpolateVarsInRemark(ItemOwnedModel model, string? remark) {
        if(remark == null) {
            return null;
        }

        bool wasSomethingNull = false;
        var resultRemark = Regex.Replace(remark, @"\{([^}]+)\}", match => {
            var propertyName = match.Groups[1].Value;
            var property = typeof(ItemOwnedModel).GetProperty(propertyName);

            if(property == null) {
                return match.Value;
            }

            var value = property.GetValue(model);
            if(value == null) {
                wasSomethingNull = true;
                return string.Empty;
            }

            Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            return propertyType switch {
                Type t when t == typeof(string) => value.ToString(),
                Type t when t == typeof(int) || t == typeof(long) || t == typeof(short) => value.ToString(),
                Type t when t == typeof(float) || t == typeof(double) || t == typeof(decimal) =>
                    ((IFormattable) value).ToString("F2", null),  // Format with 2 decimal places
                Type t when t == typeof(DateTime) => ((DateTime) value).ToString("yyyy-MM-dd"),
                Type t when t == typeof(bool) => (bool) value ? "Yes" : "No",
                Type t when t == typeof(ItemRarity) => ((ItemRarity) value).ToReadableString(),
                Type t when t == typeof(ItemSource) => ((ItemSource) value).ToReadableString(),
                Type t when t == typeof(ItemType) => ((ItemType) value).ToReadableString(),
                _ => value.ToString()
            };

        });

        return wasSomethingNull ? null : resultRemark;
    }
}