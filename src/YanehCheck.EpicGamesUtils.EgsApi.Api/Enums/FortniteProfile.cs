namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;

public enum FortniteProfile {
    /// <summary>
    /// Common information. Includes banners, purchases, tokens for founders, ...
    /// </summary>
    CommonCore = 0,
    /// <summary>
    /// Battle royale.
    /// </summary>
    Athena,
    /// <summary>
    /// Save the world.
    /// </summary>
    Campaign,
    /// <summary>
    /// Public common information. Current banner and so on.
    /// </summary>
    CommonPublic,
    /// <summary>
    /// Battle royale collections.
    /// </summary>
    Collections,
    /// <summary>
    /// Save the world metadata.
    /// </summary>
    Metadata,
    CollectionBookPeople,
    CollectionBookSchematics,
    /// <summary>
    /// Save the world storage.
    /// </summary>
    Outpost0,
    /// <summary>
    /// Save the world backpack.
    /// </summary>
    Theater0,
    /// <summary>
    /// Save the world event backpack.
    /// </summary>
    Theater1,
    /// <summary>
    /// Save the world venture backpack .
    /// </summary>
    Theater2,
    /// <summary>
    /// Save the world recycle bin.
    /// </summary>
    RecycleBin
}

public static class FortniteProfileExtensions {
    public static string ToParameterString(this FortniteProfile profile) {
        return profile switch {
            FortniteProfile.CommonCore => "common_core",
            FortniteProfile.Athena => "athena",
            FortniteProfile.Campaign => "campaign",
            FortniteProfile.CommonPublic => "common_public",
            FortniteProfile.Collections => "collections",
            FortniteProfile.Metadata =>  "metadata",
            FortniteProfile.CollectionBookPeople => "collection_book_people0",
            FortniteProfile.CollectionBookSchematics => "collection_book_schematics0",
            FortniteProfile.Outpost0 => "outpost0",
            FortniteProfile.Theater0 => "theater0",
            FortniteProfile.Theater1 => "theater1",
            FortniteProfile.Theater2 => "theater2",
            FortniteProfile.RecycleBin => "recycle_bin"
        };
    }
}