namespace YanehCheck.General.InterConsoleLibrary.Colors;

public static class ConsoleColorExtensions {
    private static readonly Dictionary<InterConsoleColor, string> ForegroundColorMap = new()
    {
        { InterConsoleColor.Black, AnsiColors.Black },
        { InterConsoleColor.Red, AnsiColors.Red },
        { InterConsoleColor.Green, AnsiColors.Green },
        { InterConsoleColor.Yellow, AnsiColors.Yellow },
        { InterConsoleColor.Blue, AnsiColors.Blue },
        { InterConsoleColor.Magenta, AnsiColors.Magenta },
        { InterConsoleColor.Cyan, AnsiColors.Cyan },
        { InterConsoleColor.White, AnsiColors.White },
        { InterConsoleColor.BrightBlack, AnsiColors.BrightBlack },
        { InterConsoleColor.BrightRed, AnsiColors.BrightRed },
        { InterConsoleColor.BrightGreen, AnsiColors.BrightGreen },
        { InterConsoleColor.BrightYellow, AnsiColors.BrightYellow },
        { InterConsoleColor.BrightBlue, AnsiColors.BrightBlue },
        { InterConsoleColor.BrightMagenta, AnsiColors.BrightMagenta },
        { InterConsoleColor.BrightCyan, AnsiColors.BrightCyan },
        { InterConsoleColor.BrightWhite, AnsiColors.BrightWhite }
    };

    private static readonly Dictionary<InterConsoleColor, string> BackgroundColorMap = new()
    {
        { InterConsoleColor.Black, AnsiColors.BgBlack },
        { InterConsoleColor.Red, AnsiColors.BgRed },
        { InterConsoleColor.Green, AnsiColors.BgGreen },
        { InterConsoleColor.Yellow, AnsiColors.BgYellow },
        { InterConsoleColor.Blue, AnsiColors.BgBlue },
        { InterConsoleColor.Magenta, AnsiColors.BgMagenta },
        { InterConsoleColor.Cyan, AnsiColors.BgCyan },
        { InterConsoleColor.White, AnsiColors.BgWhite },
        { InterConsoleColor.BrightBlack, AnsiColors.BgBrightBlack },
        { InterConsoleColor.BrightRed, AnsiColors.BgBrightRed },
        { InterConsoleColor.BrightGreen, AnsiColors.BgBrightGreen },
        { InterConsoleColor.BrightYellow, AnsiColors.BgBrightYellow },
        { InterConsoleColor.BrightBlue, AnsiColors.BgBrightBlue },
        { InterConsoleColor.BrightMagenta, AnsiColors.BgBrightMagenta },
        { InterConsoleColor.BrightCyan, AnsiColors.BgBrightCyan },
        { InterConsoleColor.BrightWhite, AnsiColors.BgBrightWhite }
    };

    public static string GetAnsiForegroundColor(this InterConsoleColor color) {
        return ForegroundColorMap[color];
    }

    public static string GetAnsiBackgroundColor(this InterConsoleColor color) {
        return BackgroundColorMap[color];
    }
}