namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public static class EpicGamesServiceHelpers {

    /// <summary>
    /// Returns a list of fortnite item ID prefixes, which represent things other than items. Mostly used with QueryProfile Endpoint.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetQueryProfileRemoveFilter() {
        // The list of ID prefixes may be incomplete, but it doesn't really matter as weird item IDS
        // are removed only for lookup performance reasons. 
        return [
            "Quest", // Quests
            "ChallengeBundle", // Related to quests, probably something like "bonus goals"
            "ChallengeBundleCompletionToken", // Related to above 
            "ChallengeBundleSchedule", // Related to above 
            "AthenaSeason", // Active seasons in all gamemodes
            "RepeatableDailiesCard", // Related to above
            "Token", // Related to active seasons and quests
            "CosmeticLocker", // Items in locker, they seem kind of outdated? 
            "CosmeticLoadout", // Loadouts
            "ConditionalAction", // Some conditions mostly related to quests/seasons
            "AccountResource", // Currencies like Wristbands, Reboot points, etc.
            "VictoryCrown", // Victory crown information and status
            "GiftBox", // Has ID with something related to the Festival challenge goal rewards 
            "PlayerAugmentsPersistence", // ???
            "AthenaRewardGraph", // Has ID related to Winterfest 2021 presents
            "FriendChest", // Has ID related to S17 Invasion season

            // ----------------------------------------------------------------
            // Used for Builtin emotes, will be maybe added later
            // Emote ID under sibling "attributes:access_item"
            "ItemAccessToken",
            // ----------------------------------------------------------------

            // ----------------------------------------------------------------
            // Styles, will be maybe added later
            "CosmeticVariantToken",
            // ----------------------------------------------------------------

            // ----------------------------------------------------------------
            // The banners fetched are incomplete
            // I will look for another endpoint before including them here
            // Most banners are listed 2 times with different prefixes and slightly different IDs in A grants B relationship
            // You can see the duplicates even on Fortnite.GG
            "HomebaseBannerIcon",
            "BannerToken",
            "ItemAccessToken"
            // ----------------------------------------------------------------
        ];
        // We are keeping
        //"AthenaDance", "AthenaSkyDiveContrail", "AthenaGlider",
        //"SparksSong", "AthenaCharacter", "VehicleCosmetics_Skin",
        //"AthenaPickaxe", "AthenaLoadingScreen", "SparksGuitar",
        //"VehicleCosmetics_DriftTrail", "AthenaBackpack"," VehicleCosmetics_Wheel",
        //"AthenaItemWrap", "JunoBuildingSet", "JunoBuildingProp",
        //"AthenaMusicPack", "SparksBass", "SparksAura",
        //"SparksMicrophone"," VehicleCosmetics_Body", "VehicleCosmetics_Booster",
        //"SparksKeyboard", "SparksDrums"
    }
}