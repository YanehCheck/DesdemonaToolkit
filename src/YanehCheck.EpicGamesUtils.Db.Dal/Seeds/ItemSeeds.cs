using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Seeds;

public static class ItemSeeds {
    public static ItemEntity ItemEntity1 = new() {
        Id = Guid.NewGuid(),
        FortniteId = "TUG_SHAKE",
        FortniteGgId = "1",
        Name = "Test name",
        PriceVbucks = 1500,
        Rarity = ItemRarity.Uncommon,
        Type = ItemType.Outfit,
        Tags = [ItemTag.Reactive],
        LastSeen = DateTime.Today,
        Release = new DateTime(2020, 4, 4),
        Description = "Test description",
        SourceDescription = "Source description",
        Styles = ["NAKED", "HOT"],
        Source = ItemSource.BattlePass,
        Set = "Test Set",
        Season = "c4s2"
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ItemEntity>().HasData(ItemEntity1);
}