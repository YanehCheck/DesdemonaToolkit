using System.Text.RegularExpressions;
using YanehCheck.General.InterConsoleLibrary.Colors;

namespace YanehCheck.General.InterConsoleLibrary;

public static class InterConsole {
    public static string InputPrefix { private get; set; } = "";
    public static string OutputPrefix { private get; set; } = "";
    public static string LongOutputPrefix { private get; set; } = "|";
    public static string ChoiceCursor { private get; set; } = ">";
    public static int ChoiceSpacing { private get; set; } = 5;
    private static int realChoiceSpacing => ChoiceSpacing > ChoiceCursor.LengthWithoutAnsiColor() ? ChoiceSpacing : ChoiceCursor.LengthWithoutAnsiColor();

    #region Basic IO

    public static void Clear() => Console.Clear();
    public static void Write(string text) => Console.Write(text);
    public static void WriteLine(string text) => Console.WriteLine(OutputPrefix+text);
    public static void WriteRawLine(string text) => Console.WriteLine(text);
    public static void WriteLongLine(string text) {
        var lines = text.Split(Environment.NewLine);
        var modifiedLines = lines.Select((line) => LongOutputPrefix + line);
        Console.WriteLine(string.Join("\n", modifiedLines));
    }

    public static int? Read() => Console.Read();
    public static string? ReadLine() {
        Console.Write(InputPrefix);
        return Console.ReadLine();
    }

    public static string? ReadLine(IEnumerable<string> allowedValues) {
        Console.Write(InputPrefix);
        var value = Console.ReadLine();
        if(value is null) {
            return null;
        }

        return allowedValues.Contains(value) ? value : null;
    }

    public static string? ReadLine(Regex allowedValues) {
        Console.Write(InputPrefix);
        var value = Console.ReadLine();
        if(value is null) {
            return null;
        }

        return allowedValues.Match(value).Success ? value : null;
    }

    public static string? ReadLine(Func<string, bool> validator) {
        Console.Write(InputPrefix);
        var value = Console.ReadLine();
        if(value is null) {
            return null;
        }

        return validator(value) ? value : null;
    }

    #endregion

    #region Basic colors

    public static void SetForegroundColor(InterConsoleColor color) {
        Console.Write(color.GetAnsiForegroundColor());
    }

    public static void SetBackgroundColor(InterConsoleColor color) {
        Console.Write(color.GetAnsiBackgroundColor());
    }

    public static void ResetColor() => Console.Write(AnsiColors.Reset);

    #endregion

    #region Advanced input

    public static string PromptMultipleChoice(string promptMessage, IEnumerable<string> options, string? defaultChoice = null) {
        WriteLine(promptMessage);
        var cursorListPos = Console.CursorTop;
        var optionsList = options.ToList();
        
        defaultChoice ??= optionsList.First();
        var currentIndex = optionsList.IndexOf(defaultChoice);

        // Print the list
        foreach (var option in options) {
            if (defaultChoice == option) {
                Console.WriteLine(' '.Repeat(realChoiceSpacing - ChoiceCursor.LengthWithoutAnsiColor()) + ChoiceCursor + option);
            }
            else {
                Console.WriteLine(' '.Repeat(realChoiceSpacing) + option);
            }
        }

        // Listen for key presses
        while (true) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if(key.Key is ConsoleKey.Enter or ConsoleKey.Spacebar or ConsoleKey.RightArrow) {
                var option = optionsList.ElementAt(currentIndex);
                Console.SetCursorPosition(0, cursorListPos);
                ClearBufferCharsAfterMultipleChoice(options);
                Console.WriteLine(ChoiceCursor + option);
                return option;
            }
            else if (key.Key is ConsoleKey.UpArrow) {
                RedrawCursor(true);
            }
            else if (key.Key is ConsoleKey.DownArrow) {
                RedrawCursor(false);
            }
        }

        // Inline func to redraw cursor
        void RedrawCursor(bool up) {
            if ((up && currentIndex == 0) || (!up && currentIndex + 1 == optionsList.Count)) {
                return;
            }

            Console.SetCursorPosition(
                realChoiceSpacing - ChoiceCursor.LengthWithoutAnsiColor(),
                cursorListPos + currentIndex
                );
            Console.Write(' '.Repeat(ChoiceCursor.LengthWithoutAnsiColor()));

            currentIndex += up ? -1 : 1;

            Console.SetCursorPosition(
                realChoiceSpacing - ChoiceCursor.LengthWithoutAnsiColor(),
                cursorListPos + currentIndex
            );
            Console.Write(ChoiceCursor);
        }
    }

    private static void ClearBufferCharsAfterMultipleChoice(IEnumerable<string> options) {
        var origX = Console.CursorLeft;
        var origY = Console.CursorTop;
        for (int i = 0; i < options.Count(); i++) {
            Console.SetCursorPosition(0, origY + i);
            var option = options.ElementAt(i);
            Console.Write(new string(' ', realChoiceSpacing + option.Length));
        }
        Console.SetCursorPosition(origX, origY);
    }

    public static T PromptMultipleChoice<T>(string promptMessage, IEnumerable<string> options, Dictionary<string, T> map, string? defaultChoice = null) {
        var result = PromptMultipleChoice(promptMessage, options, defaultChoice);
        return map[result];
    }

    #endregion
}