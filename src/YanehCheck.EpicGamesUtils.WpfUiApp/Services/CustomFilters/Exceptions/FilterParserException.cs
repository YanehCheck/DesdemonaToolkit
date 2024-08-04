using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;

public class FilterParserException : Exception {
    public FilterParserException(string message) {
        Message = message;
    }
    public FilterParserException(int line, int character) {
        Message = $"Line {line}, character {character}: Unexpected error.";
    }
    public FilterParserException(int line, int character, string message) {
        Message = $"Line {line}, character {character}: {message}";
    }
    public FilterParserException(int line, int character, TokenType expected) {
        Message = $"Line {line}, character {character}: Expected token of type {Enum.GetName(expected)}.";
    }

    public FilterParserException(int line, int character, TokenType expected, TokenType actual) {
        Message = $"Line {line}, character {character}: Expected token of type {Enum.GetName(expected)}, got {Enum.GetName(actual)} instead.";
    }

    public override string Message { get; }
}