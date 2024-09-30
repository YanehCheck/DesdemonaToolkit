using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Cli;

public class CliHandler(IFortniteItemProvider itemProvider, IFortniteStyleProvider styleProvider) : ICliHandler {

    [DllImport("Kernel32")]
    private static extern void AllocConsole();

    [DllImport("Kernel32")]
    private static extern void FreeConsole();

    public void ShowConsole() => AllocConsole();
    public void CloseConsole() => FreeConsole();

    public async Task HandleAsync(string[] args) {
        switch(args[0]) {
            case "fgg-data-to-json":
                await HandleFggDataToJson(args[1..]);
                break;
            case "style-data-to-json":
                await HandleStyleDataToJson(args[1..]);
                break;
            default:
                await Console.Error.WriteLineAsync("Unknown option");
                break;
        }
    }

    private async Task HandleStyleDataToJson(string[] args) {
        var inputPath = args[0];
        var outputPath = args[1];

        var styles = await styleProvider.GetStylesPackagePropertiesFileRecursiveAsync(inputPath, p => {
            var progress = (int) (p * 100);
            if(progress % 10 == 0) {
                Console.WriteLine($"{progress}%");
            }
        });
        var json = JsonConvert.SerializeObject(styles, new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            StringEscapeHandling = StringEscapeHandling.Default
        });
        if(!Directory.Exists(outputPath)) {
            await Console.Error.WriteLineAsync("Output path leads to invalid directory");
        }

        await File.WriteAllTextAsync(outputPath, json);
    }

    private async Task HandleFggDataToJson(string[] args) {
        var path = args[0];

        var items = await itemProvider.GetItemsFortniteGgAsync(p => {
            var progress = (int) (p * 100);
            if(progress % 10 == 0) {
                Console.WriteLine($"{progress}%");
            }
        });
        var json = JsonConvert.SerializeObject(items, new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            StringEscapeHandling = StringEscapeHandling.Default
        });
        if(!Directory.Exists(path)) {
            await Console.Error.WriteLineAsync("Output path leads to invalid directory");
        }

        await File.WriteAllTextAsync(path, json);
    }
}