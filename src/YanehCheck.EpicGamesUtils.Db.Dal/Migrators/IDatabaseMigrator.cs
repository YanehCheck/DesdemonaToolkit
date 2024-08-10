namespace YanehCheck.EpicGamesUtils.Db.Dal.Migrators;

public interface IDatabaseMigrator {
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}