namespace YanehCheck.EpicGamesUtils.Common.Enums.Items;

public static class ItemEnumExtensions {
    public static ItemRarity FromStringToItemRarity(string value) {
        string formattedValue = string.Concat(value.Split(' ', '-')).ToLower();

        return formattedValue switch {
            "common" => ItemRarity.Common,
            "uncommon" => ItemRarity.Uncommon,
            "rare" => ItemRarity.Rare,
            "epic" => ItemRarity.Epic,
            "legendary" => ItemRarity.Legendary,
            "mclaren" => ItemRarity.McLaren,
            "nissan" => ItemRarity.Nissan,
            "tesla" => ItemRarity.Tesla,
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
            "alanwalker" => ItemRarity.AlanWalker,
            "bmw" => ItemRarity.Bmw,
            _ => ItemRarity.Unknown
        };
    }

    public static string ToReadableString(this ItemRarity rarity) {
        return rarity switch {
            ItemRarity.Common => "Common",
            ItemRarity.Uncommon => "Uncommon",
            ItemRarity.Rare => "Rare",
            ItemRarity.Epic => "Epic",
            ItemRarity.Legendary => "Legendary",
            ItemRarity.McLaren => "McLaren",
            ItemRarity.Nissan => "Nissan",
            ItemRarity.Tesla => "Tesla",
            ItemRarity.Lamborghini => "Lamborghini",
            ItemRarity.StarWarsSeries => "Star Wars Series",
            ItemRarity.SlurpSeries => "Slurp Series",
            ItemRarity.ShadowSeries => "Shadow Series",
            ItemRarity.MarvelSeries => "Marvel Series",
            ItemRarity.LavaSeries => "Lava Series",
            ItemRarity.IconSeries => "Icon Series",
            ItemRarity.GamingSeries => "Gaming Legends Series",
            ItemRarity.FrozenSeries => "Frozen Series",
            ItemRarity.DcSeries => "DC Series",
            ItemRarity.DarkSeries => "Dark Series",
            ItemRarity.AlanWalker => "Alan Walker",
            ItemRarity.Bmw => "BMW",
            _ => "Unknown"
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
            _ => ItemSource.Unknown
        };
    }

    public static string ToReadableString(this ItemSource source) {
        return source switch {
            ItemSource.Shop => "Shop",
            ItemSource.BattlePass => "Battle Pass",
            ItemSource.Crew => "Fortnite Crew",
            ItemSource.Challenges => "Challenges",
            ItemSource.Exclusives => "Exclusives",
            ItemSource.Packs => "Packs",
            _ => "Unknown"
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
            // TODO: Do something about this exception
            _ => throw new ArgumentException($"Unknown item tag: {value}")
        };
    }

    public static string ToReadableString(this ItemTag tag) {
        return tag switch {
            ItemTag.Forged => "Forged",
            ItemTag.Styles => "Selectable/Unlockable Styles",
            ItemTag.Reactive => "Reactive",
            ItemTag.Traversal => "Traversal",
            ItemTag.Builtin => "Built-in",
            ItemTag.Synced => "Synced Emote",
            ItemTag.Animated => "Animated",
            ItemTag.Transformation => "Transformation",
            ItemTag.Enlightened => "Enlightened",
            _ => throw new ArgumentException($"Unknown item tag: {tag}")
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
            _ => ItemType.Unknown
        };
    }

    public static string ToReadableString(this ItemType type) {
        return type switch {
            ItemType.Outfit => "Outfit",
            ItemType.Backpack => "Backbling",
            ItemType.Wrap => "Wrap",
            ItemType.Pickaxe => "Pickaxe",
            ItemType.Bundle => "Bundle",
            ItemType.Emote => "Emote",
            ItemType.Glider => "Glider",
            ItemType.LoadingScreen => "Loading Screen",
            ItemType.Music => "Music",
            ItemType.Contrail => "Contrail",
            ItemType.Spray => "Spray",
            ItemType.Emoji => "Emoji",
            ItemType.Toy => "Toy",
            ItemType.Banner => "Banner",
            ItemType.Build => "Build",
            ItemType.Decor => "Decor",
            ItemType.Car => "Car",
            ItemType.Decal => "Decal",
            ItemType.Wheels => "Wheels",
            ItemType.Trail => "Trail",
            ItemType.Boost => "Boost",
            ItemType.Jamtrack => "Jam Track",
            ItemType.Guitar => "Guitar",
            ItemType.Bass => "Bass",
            ItemType.Drums => "Drums",
            ItemType.Keytar => "Keytar",
            ItemType.Microphone => "Microphone",
            ItemType.Aura => "Aura",
            _ => "Unknown"
        };
    }
}