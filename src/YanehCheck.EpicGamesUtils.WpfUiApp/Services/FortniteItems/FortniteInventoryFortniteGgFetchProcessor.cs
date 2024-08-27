using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteInventoryFortniteGgFetchProcessor : IFortniteInventoryFortniteGgFetchProcessor {

    private const string JavascriptFetchSnippet =
        """ 
        const nums = {0};
        const requestsPerItem = {1};
        let completedRequests = 0;
        const totalRequests = nums.length * requestsPerItem;
        let isWaiting = false;
        
        console.log("%cAdding all the items...", "font-size: 30px; font-weight: bold;");
        console.log("%cThis can take a while, especially with huge inventories.", "font-size: 20px;");
        
        function wait(ms) {{
            return new Promise(resolve => setTimeout(resolve, ms));
        }}
        
        async function sendRequest(n) {{
            while (isWaiting) {{
                await wait(100);
            }}
        
            try {{
                const response = await fetch("https://fortnite.gg/locker", {{
                    headers: {{ "content-type": "application/x-www-form-urlencoded" }},
                    body: `add=${{n}}`,
                    method: "POST"
                }});
        
                if (response.status === 429) {{
                    console.log("%cRate limit hit. Pausing for 4 seconds...", "color: orange; font-weight: bold;");
                    isWaiting = true;
                    await wait(4000);
                    isWaiting = false;
                    return sendRequest(n); // Retry the request
                }}
        
                completedRequests++;
                if (completedRequests === totalRequests) {{
                    console.log("%cAll requests completed!", "color: green; font-size: 30px; font-weight: bold;");
                }}
            }} 
            catch (error) {{
                console.error("Error in request:", error);
            }}
        }}
        
        async function processItems() {{
            for (let i = 0; i < requestsPerItem; i++) {{
                for (let j = 0; j < nums.length; j++) {{
        			const n = nums[j];
                    await wait(1);
                    sendRequest(n);
                }}
            }}
        }}
        
        processItems();
        console.log(`%cSending ${{requestsPerItem}} requests per item.`, "font-style: italic;");
        """;
    public string Create(IEnumerable<ItemModel> items) {
        var ids = items.Select(i => i.FortniteGgId).ToList();
        var arrayString = "[" + string.Join(", ", ids) + "]";
        return string.Format(JavascriptFetchSnippet, arrayString, 5);
    }
}