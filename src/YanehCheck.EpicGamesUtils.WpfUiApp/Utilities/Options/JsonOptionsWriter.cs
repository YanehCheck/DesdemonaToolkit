using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options;

public class JsonOptionsWriter (
    IHostEnvironment environment,
    IConfigurationRoot configuration,
    string file)
    : IOptionsWriter<JObject> {
    public void Update(Action<JObject> callback, bool reload = true) {
        var fileProvider = environment.ContentRootFileProvider;
        var fi = fileProvider.GetFileInfo(file);

        JObject config;
        using (var readStream = fileProvider.GetFileInfo(file).CreateReadStream()) {
            using var reader = new StreamReader(readStream);
            config = JObject.Parse(reader.ReadToEnd());
        }

        callback(config);

        using (var writeStream = File.Open(fi.PhysicalPath!, FileMode.Create))
        {
            using var writer = new StreamWriter(writeStream);
            writer.Write(config.ToString());

        }
        if (reload) {
            configuration.Reload();
        }
    }
}