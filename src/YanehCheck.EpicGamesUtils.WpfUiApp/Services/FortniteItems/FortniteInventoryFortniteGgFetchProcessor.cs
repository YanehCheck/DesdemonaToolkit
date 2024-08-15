using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteInventoryFortniteGgFetchProcessor : IFortniteInventoryFortniteGgFetchProcessor {

    private const string JavascriptFetchSnippet =
        """ 
        const nums = {0};
        let completedRequests = 0;
        console.log("%cAdding all the items, please wait...", "font-size: 30px; font-weight: bold;");
        nums.forEach((n, i) => {{
            setTimeout(() => {{
                fetch("https://fortnite.gg/locker", {{
                    headers: {{ "content-type": "application/x-www-form-urlencoded" }},
                    body: `add=${{n}}`,
                    method: "POST"
                }}).then(() => {{
                    completedRequests++;
                    if (completedRequests === nums.length) {{
                        console.log("%cAll items added!", "color: green; font-size: 30px; font-weight: bold;");
                    }}
                }});
            }}, i * 2);
        }});
        """;
    public string Create(IEnumerable<ItemModel> items) {
        var ids = items.Select(i => i.FortniteGgId).ToList();
        var arrayString = "[" + string.Join(", ", ids) + "]";
        return string.Format(JavascriptFetchSnippet, arrayString);
    }
}