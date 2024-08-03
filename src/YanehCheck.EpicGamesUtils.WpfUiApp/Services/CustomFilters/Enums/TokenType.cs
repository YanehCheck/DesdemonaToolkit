namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Enums;

[Flags]
public enum TokenType {
    EndOfFile = 1,
    ListOperator = 2,
    Operator = 4,
    DoubleLiteral = 8,
    DateLiteral = 16,
    StringLiteral = 32,
    NullLiteral = 64,
    IntLiteral = 128,
    ItemSourceEnumLiteral = 256,
    ItemRarityEnumLiteral = 512,
    ItemTypeEnumLiteral = 1024,
    ItemTagEnumLiteral = 2048,
    ListOpen = 4096,
    ListClose = 8192,
    ListNextItem = 16384,
    Header = 32768,
    // FREE = 65536,
    // FREE = 131072,
    Property = 262144,
    And = 524288,
    Or = 1048576
}

public static class TokenTypeExtensions {
    public static TokenType AnyLiteral() =>
        TokenType.IntLiteral | TokenType.DoubleLiteral | TokenType.StringLiteral | TokenType.NullLiteral |
        TokenType.DateLiteral | TokenType.ItemSourceEnumLiteral | TokenType.ItemRarityEnumLiteral |
        TokenType.ItemTypeEnumLiteral | TokenType.ItemTagEnumLiteral;

    public static bool IsLiteral(TokenType token) =>
        (int) token >= (int) TokenType.DoubleLiteral &&
        (int) token <= (int) TokenType.ItemTagEnumLiteral;
}