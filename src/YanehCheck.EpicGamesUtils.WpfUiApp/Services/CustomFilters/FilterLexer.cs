using System.Globalization;
using System.Text.RegularExpressions;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

internal class FilterLexer {
    private string sourceString;
    public int Line { get; private set; } = 0;
    public int Char { get; private set; } = 0;
    public int Pos { get; private set; } = 0;

    private Stack<Token> pushedBackTokens = new();

    private Dictionary<TokenType, Func<Token?>> expectToFunc;

    public FilterLexer(string @string) {
        sourceString = @string;
        expectToFunc = new Dictionary<TokenType, Func<Token?>>
        {
            { TokenType.EndOfFile, LexEndOfFile },
            { TokenType.ListOperator, LexListOperator },
            { TokenType.Operator, LexOperator },
            { TokenType.DoubleLiteral, LexDoubleLiteral },
            { TokenType.IntLiteral, LexIntLiteral },
            { TokenType.StringLiteral, LexStringLiteral },
            { TokenType.NullLiteral, LexNullLiteral },
            { TokenType.DateLiteral, LexDateLiteral },
            { TokenType.ItemSourceEnumLiteral, () => LexEnumLiteral<ItemSource>(TokenType.ItemSourceEnumLiteral) },
            { TokenType.ItemRarityEnumLiteral, () => LexEnumLiteral<ItemRarity>(TokenType.ItemRarityEnumLiteral) },
            { TokenType.ItemTypeEnumLiteral,() => LexEnumLiteral<ItemType>(TokenType.ItemTypeEnumLiteral) },
            { TokenType.ItemTagEnumLiteral, () => LexEnumLiteral<ItemTag>(TokenType.ItemTagEnumLiteral) },
            { TokenType.ListOpen, LexListOpen },
            { TokenType.ListClose, LexListClose },
            { TokenType.ListNextItem, LexListNextItem },
            { TokenType.Header, LexHeader },
            { TokenType.Property, LexProperty },
            { TokenType.And, LexAnd },
            { TokenType.Or, LexOr }
        };
    }

    public Token? GetNextToken(TokenType expecting) {
        if(pushedBackTokens.Count > 0) {
            return pushedBackTokens.Pop();
        }

        while(true) {
            foreach(TokenType tokenType in Enum.GetValues(typeof(TokenType))) {
                if(expecting.HasFlag(tokenType) && expectToFunc.TryGetValue(tokenType, out var lexFunc)) {
                    var token = lexFunc();
                    if(token != null) {
                        return token;
                    }
                }
            }

            if(sourceString[Pos] is ' ' or '\t' or '\r' or '\v') {
                Pos++;
                Char++;
                continue;
            }

            if(sourceString[Pos] is '\n') {
                Pos++;
                Line++;
                Char = 0;
                continue;
            }

            return null;
        }
    }

    public void PushBack(Token token) => pushedBackTokens.Push(token);
    private void UpdatePos(int by) {
        Char += by;
        Pos += by;
    }

    private Token? LexEndOfFile() {
        if(Pos == sourceString.Length) {
            return new Token(TokenType.EndOfFile);
        }

        return null;
    }

    private Token? LexProperty() {
        var properties = typeof(ItemModel).GetProperties()
            .Where(p => p.Name != "Id");
        var property = properties.SingleOrDefault(n => sourceString[Pos..].StartsWith(n.Name));
        if(property != null) {
            UpdatePos(property.Name.Length);
            return new Token(TokenType.Property, property.Name);
        }
        return null;
    }
    private Token? LexListOperator() {
        var str = sourceString[Pos..];

        if(str.StartsWith('@')) {
            UpdatePos(1);
            return new Token(TokenType.ListOperator, ListOperation.All);
        }
        if(str.StartsWith("!@")) {
            UpdatePos(2);
            return new Token(TokenType.ListOperator, ListOperation.NotAll);
        }
        if(str.StartsWith('*')) {
            UpdatePos(1);
            return new Token(TokenType.ListOperator, ListOperation.Any);
        }
        if(str.StartsWith("!*")) {
            UpdatePos(2);
            return new Token(TokenType.ListOperator, ListOperation.NotAny);
        }
        return null;
    }

    private Token? LexOperator() {
        var str = sourceString[Pos..];

        if(str.StartsWith("{")) {
            UpdatePos(1);
            return new Token(TokenType.Operator, Operation.Contains);
        }
        if(str.StartsWith("!{")) {
            UpdatePos(2);
            return new Token(TokenType.Operator, Operation.NotContains);
        }
        if(str.StartsWith("===")) {
            UpdatePos(3);
            return new Token(TokenType.Operator, Operation.StrictEquals);
        }
        if(str.StartsWith("!==")) {
            UpdatePos(3);
            return new Token(TokenType.Operator, Operation.StrictNotEquals);
        }
        if(str.StartsWith("==")) {
            UpdatePos(2);
            return new Token(TokenType.Operator, Operation.Equals);
        }
        if(str.StartsWith("!=")) {
            UpdatePos(2);
            return new Token(TokenType.Operator, Operation.NotEquals);
        }
        if(str.StartsWith(">=")) {
            UpdatePos(2);
            return new Token(TokenType.Operator, Operation.GreaterThanOrEqual);
        }
        if(str.StartsWith(">")) {
            UpdatePos(1);
            return new Token(TokenType.Operator, Operation.GreaterThan);
        }
        if(str.StartsWith("<=")) {
            UpdatePos(2);
            return new Token(TokenType.Operator, Operation.LessThanOrEqual);
        }
        if(str.StartsWith('<')) {
            UpdatePos(1);
            return new Token(TokenType.Operator, Operation.LessThan);
        }

        return null;
    }
    private Token? LexIntLiteral() {
        var match = Regex.Match(sourceString[Pos..], @"^(?<int>[0-9]+)");
        if(match.Success) {
            if(sourceString[Pos + match.Length] is '-' or '.') {
                return null;
            }

            UpdatePos(match.Length);
            return new Token(TokenType.IntLiteral, int.Parse(match.Groups["int"].Value));
        }
        return null;
    }
    private Token? LexDoubleLiteral() {
        var match = Regex.Match(sourceString[Pos..], @"^(?<double>[0-9]+\.[0-9]+)");
        if(match.Success) {
            UpdatePos(match.Length);
            return new Token(TokenType.DoubleLiteral, decimal.Parse(match.Groups["double"].Value, CultureInfo.InvariantCulture));
        }
        return null;
    }
    private Token? LexStringLiteral() {
        var match = Regex.Match(sourceString[Pos..], @"^""(?<string>[^""\n]*)""");
        if(match.Success) {
            UpdatePos(match.Length);
            return new Token(TokenType.StringLiteral, match.Groups["string"].Value);
        }
        return null;
    }

    private Token? LexNullLiteral() {
        if(sourceString[Pos..].StartsWith("null")) {
            UpdatePos(4);
            return new Token(TokenType.NullLiteral);
        }

        return null;
    }

    private Token? LexDateLiteral() {
        var match = Regex.Match(sourceString[Pos..], @"^(?<date>[0-9]{4}-[0-9]{1,2}-[0-9]{1,2})");
        if(match.Success) {
            var dateString = match.Groups["date"].Value;
            if(DateTime.TryParse(dateString, out var date)) {
                UpdatePos(match.Length);
                return new Token(TokenType.DateLiteral, date);
            }

            return null;
        }
        return null;
    }

    private Token? LexEnumLiteral<T>(TokenType enumTokenType) where T : Enum {
        var names = Enum.GetNames(typeof(T));
        var stringValue = names.SingleOrDefault(n => sourceString[Pos..].StartsWith(n));
        if(stringValue != null) {
            UpdatePos(stringValue.Length);
            return new Token(enumTokenType, Enum.Parse(typeof(T), stringValue));
        }
        return null;
    }
    private Token? LexListOpen() {
        if(sourceString[Pos..].StartsWith('[')) {
            UpdatePos(1);
            return new Token(TokenType.ListOpen);
        }

        return null;
    }

    private Token? LexListClose() {
        if(sourceString[Pos..].StartsWith(']')) {
            UpdatePos(1);
            return new Token(TokenType.ListClose);
        }

        return null;
    }


    private Token? LexListNextItem() {
        if(sourceString[Pos..].StartsWith(',')) {
            UpdatePos(1);
            return new Token(TokenType.ListNextItem);
        }

        return null;
    }

    private Token? LexHeader() {
        var match = Regex.Match(sourceString[Pos..], @"^###(?<header>.+)=(?<value>.+)");
        if(match.Success) {
            var header = match.Groups["header"].Value.Trim();
            if(Enum.TryParse(header, true, out HeaderType headerEnum)) {
                var value = match.Groups["value"].Value.Trim();
                UpdatePos(match.Length);
                return new Token(TokenType.Header, new HeaderInformation(headerEnum, value));
            }

            return null;
        }
        return null;
    }

    private Token? LexAnd() {
        if(sourceString[Pos..].StartsWith("&&")) {
            UpdatePos(2);
            return new Token(TokenType.And);
        }

        return null;
    }

    private Token? LexOr() {
        if(sourceString[Pos..].StartsWith("||")) {
            UpdatePos(2);
            return new Token(TokenType.Or);
        }

        return null;
    }
}