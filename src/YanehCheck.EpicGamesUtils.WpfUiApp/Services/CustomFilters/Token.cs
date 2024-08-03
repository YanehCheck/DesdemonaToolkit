using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

/// <summary>
/// Value should be boxed target type.
/// </summary>
public class Token(TokenType type, object? value = null) : Token<object?>(type, value);
public class Token<T>(TokenType type, T? value = null) where T : class? {
    public TokenType Type { get; init; } = type;
    public T? Value { get; init; } = value;
}