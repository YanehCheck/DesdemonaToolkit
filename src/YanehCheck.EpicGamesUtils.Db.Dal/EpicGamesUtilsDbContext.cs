using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Dal;

public class EpicGamesUtilsDbContext : DbContext {
    public EpicGamesUtilsDbContext(DbContextOptions options, bool seedData = false) : base(options) { }
    public DbSet<ItemEntity> ItemEntities => Set<ItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ItemEntity>()
            .Property(e => e.FortniteId)
            .HasConversion(v => v.ToLowerInvariant(), 
                v => v);
        modelBuilder.Entity<ItemEntity>()
            .Navigation(i => i.Styles)
            .AutoInclude();
        modelBuilder.Entity<ItemEntity>()
            .Property(e => e.FortniteGgStyles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        modelBuilder.Entity<ItemEntity>()
            .Property(e => e.Tags)
            .HasConversion(
                v => string.Join(',', v.Select(x => (int) x)),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => (ItemTag) Convert.ToInt32(x)));

        modelBuilder.Entity<ItemStyleEntity>()
            .Property(e => e.ItemFortniteId)
            .HasConversion(v => v.ToLowerInvariant(),
                v => v);
        modelBuilder.Entity<ItemStyleEntity>()
            .Property(e => e.FortniteId)
            .HasConversion(v => v.ToLowerInvariant(),
                v => v);
        modelBuilder.Entity<ItemEntity>()
            .HasMany(e => e.Styles)
            .WithOne(e => e.Item)
            .HasPrincipalKey(e => e.FortniteId);
        modelBuilder.Entity<ItemStyleEntity>()
            .HasOne(e => e.Item)
            .WithMany(e => e.Styles)
            .HasForeignKey(e => e.ItemFortniteId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}