using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Migrators;

public class DatabaseMigrator(IDbContextFactory<EpicGamesUtilsDbContext> dbContextFactory, IOptions<DalOptions> options) : IDatabaseMigrator {
    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

    public async Task MigrateAsync(CancellationToken cancellationToken) {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        if(options.Value.ResetDatabase) {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        Directory.CreateDirectory(options.Value.DatabaseDirectory);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}