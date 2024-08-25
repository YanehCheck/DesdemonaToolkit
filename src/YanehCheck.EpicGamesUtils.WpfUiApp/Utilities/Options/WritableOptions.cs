using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options;

public class WritableOptions<T>(string sectionPath, IOptionsWriter<JObject> writer, IOptionsMonitor<T> optionsMonitor) : IWritableOptions<T> where T : class, new() {
    public T Value => optionsMonitor.CurrentValue;

    public void Update(Action<T> applyChanges) {
        writer.Update(config => {
            var sectionObject = config.TryGetValue(sectionPath, out JToken? sectionValue) ?
                JsonConvert.DeserializeObject<T>(sectionValue.ToString()) :
                optionsMonitor.CurrentValue ?? new T();

            applyChanges(sectionObject!);

            // We need to manually get the specific section, so
            // it doesn't just create object with the literal path name in root
            var json = JsonConvert.SerializeObject(sectionObject);
            var configSection = config.SelectToken($"..{sectionPath}")!;
            configSection.Parent!.Parent![sectionPath.Split(':').Last()] = JToken.Parse(json);
        });
    }
}