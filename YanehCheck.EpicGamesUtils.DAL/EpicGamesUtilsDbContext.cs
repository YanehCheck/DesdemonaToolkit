using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.DAL;

public class EpicGamesUtilsDbContext(DbContextOptions options, bool seedData = false) : DbContext(options) {
    public DbSet<ItemEntity> ItemEntities => Set<ItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}