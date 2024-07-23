namespace YanehCheck.EpicGamesUtils.DAL;

public class DalOptions {
    public string DatabaseName { get; set; }
    public string DatabaseDirectory { get; set; }
    public string DatabasePath => Path.Combine(DatabaseDirectory, DatabaseName);
    public bool ResetDatabase { get; set; }
    public bool SeedDatabase { get; set; }
}