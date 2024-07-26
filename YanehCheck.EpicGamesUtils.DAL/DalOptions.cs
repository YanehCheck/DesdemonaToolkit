namespace YanehCheck.EpicGamesUtils.DAL;

public class DalOptions {
    public const string Key = "DAL";

    public string DatabaseName { get; set; } = "desdemonatoolkit.db";

    public string DatabaseDirectory { get; set; } =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public string DatabasePath => Path.Combine(DatabaseDirectory, DatabaseName);
    public bool ResetDatabase { get; set; } = false;
    public bool SeedDatabase { get; set; } = false;
}