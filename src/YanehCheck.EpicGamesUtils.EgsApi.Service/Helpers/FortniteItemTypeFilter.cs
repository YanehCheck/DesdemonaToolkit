namespace YanehCheck.EpicGamesUtils.EgsApi.Service.Helpers;

public static class FortniteItemTypeFilter
{

    /// <summary>
    /// Returns a list of fortnite item ID prefixes, which represent things other than items. Mostly used with QueryProfile Endpoint.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetQueryProfileRemoveFilter()
    {
        // The list of ID prefixes may be incomplete, but it doesn't really matter as weird item IDS
        // are removed only for lookup performance reasons. 
        return [
            "Quest", // Quests
            "ChallengeBundle", // Related to quests, probably something like "bonus goals"
            "ChallengeBundleCompletionToken", // Related to above 
            "ChallengeBundleSchedule", // Related to above 
            "AthenaSeason", // Active seasons in all gamemodes
            "RepeatableDailiesCard", // Related to above
            "Token", // Related to "ownership" of seasons, passes, quests, founders...
            "CosmeticLocker", // Items in locker, they seem kind of outdated? 
            "CosmeticLoadout", // Loadouts
            "ConditionalAction", // Some conditions mostly related to quests/seasons
            "AccountResource", // Currencies like Wristbands, Reboot points, etc.
            "VictoryCrown", // Victory crown information and status
            "GiftBox", // Has ID with something related to the Festival challenge goal rewards 
            "PlayerAugmentsPersistence", // ???
            "AthenaRewardGraph", // Has ID related to Winterfest 2021 presents
            "FriendChest", // Has ID related to S17 Invasion season
            "Currency", // Some probably vbucks related information when querying Common profile
            "HomebaseBannerColor", // Owned banner color
            "EventPurchaseTracker", // ??? (Common profile)
            "ItemAccessToken", // Used for adding builtin emotes

            // ----------------------------------------------------------------
            // Styles, implemented differently
            "CosmeticVariantToken",
            // ----------------------------------------------------------------

            // ----------------------------------------------------------------
            // Most banners are listed 2 times with different prefixes and slightly different IDs in A grants B relationship
            // You can see the duplicates even on Fortnite.GG
            "BannerToken"
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