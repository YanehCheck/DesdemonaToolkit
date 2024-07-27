using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;

namespace YanehCheck.EpicGamesUtils.DAL;

public class EpicGamesUtilsDbContext : DbContext {
    public EpicGamesUtilsDbContext(DbContextOptions options, bool seedData = false) : base(options) {
        Database.EnsureCreated();
    }
    public DbSet<ItemEntity> ItemEntities => Set<ItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ItemEntity>()
            .Property(e => e.Styles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        modelBuilder.Entity<ItemEntity>()
            .Property(e => e.Tags)
            .HasConversion(
                v => string.Join(',', v.Select(x => (int) x)),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => (ItemTag) Convert.ToInt32(x)));
    }
}