namespace YanehCheck.EpicGamesUtils.Common.Enums.Items;

public class ItemEnumExtensions {
    public static ItemRarity FromStringToItemRarity(string value) {
        string formattedValue = string.Concat(value.Split(' ', '-')).ToLower();

        return formattedValue switch {
            "common" => ItemRarity.Common,
            "uncommon" => ItemRarity.Uncommon,
            "rare" => ItemRarity.Rare,
            "epic" => ItemRarity.Epic,
            "legendary" => ItemRarity.Legendary,
            "mclaren" => ItemRarity.McLaren,
            "lamborghini" => ItemRarity.Lamborghini,
            "starwarsseries" => ItemRarity.StarWarsSeries,
            "slurpseries" => ItemRarity.SlurpSeries,
            "shadowseries" => ItemRarity.ShadowSeries,
            "marvelseries" => ItemRarity.MarvelSeries,
            "lavaseries" => ItemRarity.LavaSeries,
            "iconseries" => ItemRarity.IconSeries,
            "gaminglegendsseries" => ItemRarity.GamingSeries,
            "frozenseries" => ItemRarity.FrozenSeries,
            "dcseries" => ItemRarity.DcSeries,
            "darkseries" => ItemRarity.DarkSeries,
            _ => throw new ArgumentException($"Unknown item rarity: {value}")
        };
    }

    public static ItemSource FromStringToItemSource(string value) {
        string formattedValue = string.Concat(value.Split(' ', '-')).ToLower();

        return formattedValue switch {
            "shop" => ItemSource.Shop,
            "battlepass" => ItemSource.BattlePass,
            "fortnitecrew" => ItemSource.Crew,
            "challenges" => ItemSource.Challenges,
            "exclusives" => ItemSource.Exclusives,
            "packs" => ItemSource.Packs,
            _ => throw new ArgumentException($"Unknown item source: {value}")
        };
    }

    public static ItemTag FromStringToItemTag(string value) {
        string formattedValue = string.Concat(value.Split(' ', '-', '[', ']')).ToLower();

        return formattedValue switch {
            "forged" => ItemTag.Forged,
            "selectablestyles" or
            "unlockablestyles" => ItemTag.Styles,
            "reactive" => ItemTag.Reactive,
            "traversal" => ItemTag.Traversal,
            "builtin" => ItemTag.Builtin,
            "syncedemote" => ItemTag.Synced,
            "animated" => ItemTag.Animated,
            "transformation" => ItemTag.Transformation,
            "enlightened" => ItemTag.Enlightened,
            _ => throw new ArgumentException($"Unknown item tag: {value}")
        };
    }

    public static ItemType FromStringToItemType(string value) {
        string formattedValue = string.Concat(value.Split(' ', '-')).ToLower();

        return formattedValue switch {
            "outfit" => ItemType.Outfit,
            "backpack" => ItemType.Backpack,
            "wrap" => ItemType.Wrap,
            "pickaxe" => ItemType.Pickaxe,
            "bundle" => ItemType.Bundle,
            "emote" => ItemType.Emote,
            "glider" => ItemType.Glider,
            "loadingscreen" => ItemType.LoadingScreen,
            "music" => ItemType.Music,
            "contrail" => ItemType.Contrail,
            "spray" => ItemType.Spray,
            "emoji" => ItemType.Emoji,
            "toy" => ItemType.Toy,
            "banner" => ItemType.Banner,
            "build" => ItemType.Build,
            "decor" => ItemType.Decor,
            "car" => ItemType.Car,
            "decal" => ItemType.Decal,
            "wheels" => ItemType.Wheels,
            "trail" => ItemType.Trail,
            "boost" => ItemType.Boost,
            "jamtrack" => ItemType.Jamtrack,
            "guitar" => ItemType.Guitar,
            "bass" => ItemType.Bass,
            "drums" => ItemType.Drums,
            "keytar" => ItemType.Keytar,
            "microphone" => ItemType.Microphone,
            "aura" => ItemType.Aura,
            _ => throw new ArgumentException($"Unknown item type: {value}")
        };
    }
}