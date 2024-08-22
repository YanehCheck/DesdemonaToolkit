using YanehCheck.EpicGamesUtils.Utils.FestivalPlayer;

namespace TEST;

internal class Program {
    static async Task Main(string[] args) {
        FortniteFestivalPlayer player = new(new FortniteFestivalConfig {
            FifthLaneKey = 'D'
        });
        await player.Start();
        Console.ReadKey();
    }
}