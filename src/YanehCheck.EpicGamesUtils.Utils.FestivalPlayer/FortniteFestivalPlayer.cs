using System.Drawing;

namespace YanehCheck.EpicGamesUtils.Utils.FestivalPlayer;

public class FortniteFestivalPlayer(FortniteFestivalConfig config) {
    private FortniteFestivalConfig  config = config;
    private Task? playerTask;
    private CancellationTokenSource? cts;

    public async Task Start() {
        cts = new CancellationTokenSource();
        playerTask = Task.Run(() => Play(cts.Token));
    }

    public async Task Stop() {
        await cts.CancelAsync();
    }

    private void Play(CancellationToken token) {
        int height = 50;
        int width = 500;

        using Bitmap bmp = new Bitmap(width, height);
        using Graphics g = Graphics.FromImage(bmp);

        //while (true) {
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(width, height));
        //}
        bmp.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "img.bmp"));
    }
}

public record FortniteFestivalConfig {
    public char FirstLaneKey { get; set; }
    public char SecondLaneKey { get; set; }
    public char ThirdLaneKey { get; set; }
    public char FourthLaneKey { get; set; }
    public char FifthLaneKey { get; set; }
    public bool IsExpert { get; set; }
}