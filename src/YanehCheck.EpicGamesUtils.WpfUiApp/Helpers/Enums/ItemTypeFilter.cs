using YanehCheck.EpicGamesUtils.Common.Enums.Items;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Enums;

public enum ItemTypeFilter
{
    // Follows structure of (ItemType type - 1)
    Outfit, // 0
    Backpack,
    Wrap,
    Pickaxe,
    Bundle,
    Emote,
    Glider,
    LoadingScreen,
    Music,
    Contrail,
    Spray,
    Emoji,
    Toy,
    Banner, // 13

    Build, // 14
    Decor, // 15

    Car, // 16
    Decal,
    Wheels,
    Trail,
    Boost, // 20

    Jamtrack, // 21
    Guitar, // 22
    Bass,
    Drums,
    Keytar,
    Microphone, // 26
    Aura, // 27

    All = 100,
    AllBattleRoyale,
    AllLego,
    AllRocketRacing,
    AllFestival,
    Instruments,

    None = 1000
}

public static class ItemTypeFilterExtensions
{
    public static bool Satisfied(this ItemTypeFilter filter, ItemType itemType)
    {
        var filterN = (int)filter;
        var itemTypeN = (int)itemType - 1;

        if (filter == ItemTypeFilter.All)
        {
            return true;
        }

        if (filterN == itemTypeN)
        {
            return true;
        }

        return filter switch
        {
            ItemTypeFilter.AllBattleRoyale => itemTypeN is >= 0 and <= 13,
            ItemTypeFilter.AllLego => itemTypeN is 14 or 15,
            ItemTypeFilter.AllRocketRacing => itemTypeN is >= 16 and <= 20,
            ItemTypeFilter.AllFestival => itemTypeN is >= 21 and <= 27,
            ItemTypeFilter.Instruments => itemTypeN is >= 22 and <= 26,
            _ => false
        };
    }
}