namespace YanehCheck.EpicGamesUtils.DAL;

public class DalOptions {
    public const string Dal = "DAL";

    public string DatabaseName { get; set; } = "desdemonatoolkit.db";

    public string DatabaseDirectory { get; set; } =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public string DatabasePath => Path.Combine(DatabaseDirectory, DatabaseName);
    public bool ResetDatabase { get; set; } = false;
    public bool SeedDatabase { get; set; } = false;
}