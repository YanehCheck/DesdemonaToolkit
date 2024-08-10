using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;

public class FortniteGgScrapper : IFortniteGgScrapper {
    // TODO: Put this into DI
    private static readonly HttpClient Client = new(); 
    public async Task<List<FortniteGgItem>> ScrapIdRangeAsync(int from, int to) {
        var items = new List<FortniteGgItem>();
        for(int i = from; i <= to; i++) {
            try {
                var item = await ScrapItemAsync(i.ToString());
                if(item != null) {
                    items.Add(item);
                }
            }
            catch(Exception ex) {
                if(ex is ArgumentException) {
                    // TODO: Log, even though it is prob some weird item
                }
            }
        }
        return items;
    }

    public async Task<ConcurrentBag<FortniteGgItem>> ScrapIdRangeParallelAsync(int from, int to) {
        return await ScrapIdRangeParallelAsync(from, to, null);
    }

    public async Task<ConcurrentBag<FortniteGgItem>> ScrapIdRangeParallelAsync(int from, int to, Action<double>? progressReport) {
        var items = new ConcurrentBag<FortniteGgItem>();
        int progress = 0;
        await Parallel.ForEachAsync(Enumerable.Range(from, to), async (i, _) => {
            try {
                var item = await ScrapItemAsync(i.ToString());
                if(item != null) {
                    items.Add(item);
                    progress++;
                    if (progress % 10 == 0) {
                        progressReport?.Invoke((double)progress/(to-from));
                    }
                }
            }
            catch(Exception ex) {
                if(ex is ArgumentException) {
                    // TODO: Log, even though it is prob some weird item
                }
            }
        });
        return items;
    }

    public async Task<FortniteGgItem?> ScrapItemAsync(string id) {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://fortnite.gg/item-details?id={id}");
        request.Headers.Add("User-Agent", "Other");
        request.Headers.Add("Referer", $"https://fortnite.gg/item-details?id={id}");
        request.Headers.Add("Accept", "*/*");
        var response = await Client.SendAsync(request);
        if(response.StatusCode == HttpStatusCode.NotFound) {
            return null;
        }

        var document = new HtmlDocument();
        document.Load(await response.Content.ReadAsStreamAsync());
        var item = await ParseItemFromDom(document, id);
        return item;
    }

    private async Task<FortniteGgItem?> ParseItemFromDom(HtmlDocument body, string id) {
        FortniteGgItem item = new() {
            FortniteGgId = id
        };

        ParseItemNameFromDom(body, item);
        ParseRarityAndTypeFromDom(body, item);
        ParseItemSourceAndDatesFromDom(body, item);
        ParsePriceOrSourceDescriptionFromDom(body, item);
        ParseItemIdFromDom(body, item);
        ParseItemDescAndTagsFromDom(body, item);
        ParseItemSetFromDom(body, item);
        ParseItemStylesFromDom(body, item);

        return item;
    }

    private bool ParseItemStylesFromDom(HtmlDocument body, FortniteGgItem item) {
        var nodes = body.DocumentNode.SelectNodes("//*[@data-idx]");
        if(nodes == null || nodes.Count == 0) {  return true; }
        item.Styles = nodes.Select(
            n => new string(
                n.Attributes["onclick"].Value
            .SkipWhile(c => c != '"')
            .Skip(1)
            .TakeWhile(c => c != '"') 
            .ToArray())).ToList();
        if (item.Styles.Count() == 2 && item.Styles.Contains("WEAPON") && item.Styles.Contains("OUTFIT")){
            item.Styles = null!;
        }
        return true;
    }

    private bool ParseItemSetFromDom(HtmlDocument body, FortniteGgItem item) {
        var node = body.DocumentNode.SelectSingleNode("//*[.='Part of the ']")?.NextSibling;
        if(node != null) {
            item.Set = node.InnerText.Trim();
        }

        return true;
    }

    private bool ParseItemDescAndTagsFromDom(HtmlDocument body, FortniteGgItem item) {
        var nodes = body.DocumentNode.SelectNodes("//*[@class='fn-detail-desc grey']");
        if(nodes != null && nodes.Count != 0) {
            foreach (var node in nodes) {
                if (node.InnerText[0] == '[') {
                    item.Tags ??= new List<ItemTag>();
                    item.Tags = node.InnerText.Split(',').Select(tag =>
                        ItemEnumExtensions.FromStringToItemTag(tag.Trim()));
                }
                else {
                    item.Description = node.InnerText;
                }
            }
        }

        return true;
    }

    private bool ParseItemIdFromDom(HtmlDocument body, FortniteGgItem item) {
        var node = body.DocumentNode.SelectSingleNode("//*[.='ID:']")?.NextSibling;
        if (node != null) {
            item.Id = node.InnerText.Trim();
        }
        return node != null;
    }

    private bool ParsePriceOrSourceDescriptionFromDom(HtmlDocument body, FortniteGgItem item) {
        var node = body.DocumentNode.SelectSingleNode("//*[@class='fn-item-price']");

        item.PriceUsd = null;
        item.PriceVbucks = null;
        item.SourceDescription = null;

        if(node != null) {
            if(node.InnerHtml.Contains("vbucks")) {
                // Get the price in vbucks
                var vbucksTextParts = node.ChildNodes[1].InnerText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                item.PriceVbucks = Convert.ToInt32(vbucksTextParts[0].Replace(",", ""));
                // Check if there is some more information (from which skin is the backbling from, ect)
                if(node.ChildNodes.Count > 2) {
                    item.SourceDescription = node.ChildNodes[2].InnerText;
                }
            }
            else if(node.InnerHtml[0] == '$') {
                // Get the price in USD
                var usdTextParts = node.ChildNodes[0].InnerText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                item.PriceUsd = Convert.ToDecimal(usdTextParts[0].Replace("$", ""), new CultureInfo("en-US"));
                // Check if there is some more info
                if(node.ChildNodes.Count > 2) {
                    item.SourceDescription += node.ChildNodes[1].InnerText;
                }
                // Special case for info without links, trim "(" and ")"
                else if(usdTextParts.Length > 1) {
                    item.SourceDescription = string.Join(' ', usdTextParts[1..])[1..^1];
                }
            }
            else {
                // Should be fine for the rest
                item.SourceDescription = node.InnerText.Trim();
            }
        }

        return true;
    }

    private bool ParseItemSourceAndDatesFromDom(HtmlDocument body, FortniteGgItem item) {
        var nodeSource = body.DocumentNode.SelectSingleNode("//*[.='Source:']")?.NextSibling;
        var nodeSeason = body.DocumentNode.SelectSingleNode("//*[.='Introduced in:']")?.NextSibling;
        var nodeLastSeen = body.DocumentNode.SelectSingleNode("//*[.='Last seen:']")?.NextSibling;
        var nodeRelease = body.DocumentNode.SelectSingleNode("//*[.='Release date:']")?.NextSibling;

        if(nodeSource != null) { // Allowed to be empty
            item.Source = ItemEnumExtensions.FromStringToItemSource(nodeSource.InnerHtml);
        }
        else {
            item.Source = ItemSource.Unknown;
        }

        if(nodeSeason != null) {
            item.Season = ConvertSeasonNameToCompact(nodeSeason.InnerText);
        }

        if(nodeLastSeen != null) { // Allowed to be empty
            var date = nodeLastSeen.InnerText.Split('(')[0].TrimEnd();
            item.LastSeen = DateTime.Parse(date);
        }
        else {
            item.LastSeen = null;
        }

        if(nodeRelease != null) { // Allowed to be empty
            var date = nodeRelease.InnerText.Split('(')[0].TrimEnd();
            item.Release = DateTime.Parse(date);
        }
        else {
            item.Release = null;
        }

        return nodeSeason != null;
    }

    private bool ParseItemNameFromDom(HtmlDocument body, FortniteGgItem item) {
        var node = body.DocumentNode.SelectSingleNode("//*[@class='fn-detail-name']");
        if(node != null) {
            item.Name = node.InnerText;
        }
        return node != null;
    }
    private bool ParseRarityAndTypeFromDom(HtmlDocument body, FortniteGgItem item) {
        var node = body.DocumentNode.SelectSingleNode("//*[@class='fn-detail-type']");
        if(node != null && node.ChildNodes.Count == 2) {
            item.Rarity = ItemEnumExtensions.FromStringToItemRarity(node.FirstChild.InnerText);
            item.Type = ItemEnumExtensions.FromStringToItemType(node.LastChild.InnerText);
        }
        return node != null && node.ChildNodes.Count == 2;
    }

    private string ConvertSeasonNameToCompact(string season) {
        // Will need to be updated anyway in the future for second OG season, chapter XX or whatever funny stuff they come up with.
        // So no need to go overboard with some regex vars.

        // Season OG edge case
        if(season == "Season OG") {
            return "c4sOG";
        }
        // C1 edge cases
        else if(Regex.IsMatch(season, "^Season ([1-9]|X)")) {
            return $"c1s{season[7]}";
        }
        else {
            return $"c{season[8]}s{season[18]}";
        }
    }
}