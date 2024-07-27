namespace YanehCheck.EpicGamesUtils.DAL.Migrators;

public interface IDatabaseMigrator {
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}