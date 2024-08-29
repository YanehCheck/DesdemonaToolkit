using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Dtos;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Interfaces;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer;

public class FortniteAssetSerializer : IFortniteAssetSerializer {

    public ItemStyleAssetDto? DeserializeCosmeticVariantAssetFile(string file) {
        var text = File.ReadAllText(file);
        return DeserializeCosmeticVariantAsset(text);
    }
    public async Task<ItemStyleAssetDto?> DeserializeCosmeticVariantAssetFileAsync(string file) {
        var text = await File.ReadAllTextAsync(file);
        return DeserializeCosmeticVariantAsset(text);
    }

    public ItemStyleAssetDto? DeserializeCosmeticVariantAsset(string content) {
        var json = JToken.Parse(content);
        var dto = new ItemStyleAssetDto();

        dto.Name = json.SelectToken("$[0].Properties.ItemName.SourceString")?.ToObject<string>()!;
        dto.FortniteId = json.SelectToken("$[0].Name")?.ToObject<string>()!;
        dto.Description = json.SelectToken("$[0].Properties.ItemDescription.SourceString")?.ToObject<string>();
        dto.ShortDescription = json.SelectToken("$[0].Properties.ItemShortDescription.SourceString")?.ToObject<string>();
        var channelTag = json.SelectToken("$[0].Properties.VariantChannelTag.TagName")?.ToObject<string>()!;
        var nameTag = json.SelectToken("$[0].Properties.VariantNameTag.TagName")?.ToObject<string>()!;
        var objectName = json.SelectToken("$[0].Properties.cosmetic_item.ObjectName")?.ToObject<string>()!;

        dto.ItemFortniteId = objectName?.Split('\'', StringSplitOptions.RemoveEmptyEntries)?.Last()!;
        dto.Channel = channelTag == null! ? null! : string.Join(".", channelTag.Split('.').Skip(3));
        dto.Property = nameTag == null! ? null! : string.Join(".", nameTag.Split('.').Skip(3));

        if (dto.Property == null! || dto.Channel == null! || dto.FortniteId == null! ||
            dto.ItemFortniteId == null!) {
            return null;
        }

        return dto;
    }
}