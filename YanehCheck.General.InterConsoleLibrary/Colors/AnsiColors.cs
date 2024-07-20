namespace YanehCheck.General.InterConsoleLibrary.Colors;

public static class AnsiColors
{
    public const string Reset = "\u001b[0m";

    public const string Black = "\u001b[30m";
    public const string Red = "\u001b[31m";
    public const string Green = "\u001b[32m";
    public const string Yellow = "\u001b[33m";
    public const string Blue = "\u001b[34m";
    public const string Magenta = "\u001b[35m";
    public const string Cyan = "\u001b[36m";
    public const string White = "\u001b[37m";

    public const string BrightBlack = "\u001b[90m";
    public const string BrightRed = "\u001b[91m";
    public const string BrightGreen = "\u001b[92m";
    public const string BrightYellow = "\u001b[93m";
    public const string BrightBlue = "\u001b[94m";
    public const string BrightMagenta = "\u001b[95m";
    public const string BrightCyan = "\u001b[96m";
    public const string BrightWhite = "\u001b[97m";

    public const string BgBlack = "\u001b[40m";
    public const string BgRed = "\u001b[41m";
    public const string BgGreen = "\u001b[42m";
    public const string BgYellow = "\u001b[43m";
    public const string BgBlue = "\u001b[44m";
    public const string BgMagenta = "\u001b[45m";
    public const string BgCyan = "\u001b[46m";
    public const string BgWhite = "\u001b[47m";

    public const string BgBrightBlack = "\u001b[100m";
    public const string BgBrightRed = "\u001b[101m";
    public const string BgBrightGreen = "\u001b[102m";
    public const string BgBrightYellow = "\u001b[103m";
    public const string BgBrightBlue = "\u001b[104m";
    public const string BgBrightMagenta = "\u001b[105m";
    public const string BgBrightCyan = "\u001b[106m";
    public const string BgBrightWhite = "\u001b[107m";

    public static string ForegroundColor256(int color)
    {
        if (color < 0 || color > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(color), "Color must be between 0 and 255.");
        }

        return $"\u001b[38;5;{color}m";
    }

    public static string BackgroundColor256(int color)
    {
        if (color < 0 || color > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(color), "Color must be between 0 and 255.");
        }

        return $"\u001b[48;5;{color}m";
    }

    public static string ForegroundTrueColor(int r, int g, int b)
    {
        if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
        {
            throw new ArgumentOutOfRangeException(null, "RGB values must be between 0 and 255.");
        }

        return $"\u001b[38;2;{r};{g};{b}m";
    }

    public static string BackgroundTrueColor(int r, int g, int b)
    {
        if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
        {
            throw new ArgumentOutOfRangeException(null, "RGB values must be between 0 and 255.");
        }

        return $"\u001b[48;2;{r};{g};{b}m";
    }
}