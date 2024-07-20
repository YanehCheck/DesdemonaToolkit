using System.Text;
using System.Text.RegularExpressions;

namespace YanehCheck.General.InterConsoleLibrary;

internal static class StringExtensions {
    internal static string Repeat(this string value, int count) {
        return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
    }

    internal static int LengthWithoutAnsiColor(this string input) {
        string ansiEscapePattern = @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])";
        return Regex.Replace(input, ansiEscapePattern, "").Length;
    }

    internal static string Repeat(this char value, int count) {
        return new StringBuilder(count).Insert(0, value.ToString(), count).ToString();
    }
}