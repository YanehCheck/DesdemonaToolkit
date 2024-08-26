using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options;

public class JsonOptionsWriter(
    IHostEnvironment environment,
    IConfigurationRoot configuration,
    string file)
    : IOptionsWriter<JObject> {

    // Not ideal, but I don't think it should matter even if we somehow have multiple user configs
    private static readonly object fileLock = new();

    public void Update(Action<JObject> callback, bool reload = true) {
        var fileProvider = environment.ContentRootFileProvider;
        var fi = fileProvider.GetFileInfo(file);

        JObject config;

        lock(fileLock) {
            using(var readStream = fileProvider.GetFileInfo(file).CreateReadStream())
            using(var reader = new StreamReader(readStream)) {
                config = JObject.Parse(reader.ReadToEnd());
            }

            callback(config);

            using(var writeStream = new FileStream(fi.PhysicalPath!, FileMode.Create, FileAccess.Write, FileShare.None))
            using(var writer = new StreamWriter(writeStream)) {
                writer.Write(config.ToString());
            }

            if(reload) {
                configuration.Reload();
            }
        }
    }
}