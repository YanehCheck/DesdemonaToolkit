using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Dtos;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Interfaces;

public interface IFortniteAssetSerializer
{
    ItemStyleAssetDto? DeserializeCosmeticVariantAssetFile(string file);
    Task<ItemStyleAssetDto?> DeserializeCosmeticVariantAssetFileAsync(string file);
    ItemStyleAssetDto? DeserializeCosmeticVariantAsset(string content);
}