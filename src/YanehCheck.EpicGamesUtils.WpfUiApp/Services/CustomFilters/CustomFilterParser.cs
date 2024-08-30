using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

/// <inheritdoc cref="ICustomFilterParser"/>ilters;
/// <inheritdoc cref="ICustomFilterParser"/>
public class CustomFilterParser : ICustomFilterParser {
    private bool strict = true;
    public IFilter Parse(string filterString) {
        var lexer = new FilterLexer(filterString);
        var filter = new Filter();
        ParseHeaderOrProperty(lexer, filter);
        return filter;
    }

    private void ParseHeaderOrProperty(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Header | TokenType.Property | TokenType.PropertyHeader | TokenType.EndOfFile);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token.Type == TokenType.Header) {
            ProcessHeader(lexer, filter, token);
            ParseHeaderOrProperty(lexer, filter);
        }
        else if (token.Type == TokenType.PropertyHeader) {
            filter.AddClause();
            ProcessPropertyHeader(lexer, filter, token);
            ParsePropertyOrPropertyHeader(lexer, filter);
        }
        else if(token.Type == TokenType.Property) {
            filter.AddClause();
            ProcessProperty(filter, token);
            ParseOperatorOrListOperator(lexer, filter);
        }
        else if(token!.Type == TokenType.EndOfFile) {
            return;
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Internal error. Bad token returned.");
        }
    }

    private void ParseOperatorOrListOperator(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Operator | TokenType.ListOperator);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token!.Type == TokenType.Operator) {
            ProcessOperator(lexer, filter, token);
            ParseLiteralOrListOpen(lexer, filter);
        }
        else if(token!.Type == TokenType.ListOperator) {
            ProcessListOperator(filter, token);
            ParseOperator(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Internal error. Bad token returned.");
        }
    }

    private void ParseOperator(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Operator);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token!.Type == TokenType.Operator) {
            ProcessOperator(lexer, filter, token);
            ParseLiteralOrListOpen(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Unexpected token. Expected an operator.");
        }
    }

    private void ParseLiteralOrListOpen(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenTypeExtensions.AnyLiteral() | TokenType.ListOpen);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(TokenTypeExtensions.IsLiteral(token.Type)) {
            ProcessLiteral(lexer, filter, token);
            ParseAndOr(lexer, filter);
        }
        else if(token.Type == TokenType.ListOpen) {
            ProcessListOpen(lexer, filter);
            IEnumerable<object?> listValues = [];
            ParseLiteralList(lexer, filter, listValues);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Unexpected token. Expected a list or a literal.");
        }
    }

    private void ParseLiteralList(FilterLexer lexer, Filter filter, IEnumerable<object?> listValues) {
        var expecting = strict
            ? TokenTypeExtensions.AnyLiteral()
            : TokenTypeExtensions.AnyLiteral() | TokenType.ListClose;
        var token = lexer.GetNextToken(expecting);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(TokenTypeExtensions.IsLiteral(token.Type)) {
            ProcessListLiteral(lexer, filter, token);
            listValues = listValues.Append(token.Value);
            ParseListCloseOrListNext(lexer, filter, listValues);
        }
        else if (!strict && token.Type == TokenType.ListClose) {
            ProcessListClose(filter, listValues);
            ParseAndOr(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Unexpected token. Expected a literal.");
        }
    }

    private void ParseListCloseOrListNext(FilterLexer lexer, Filter filter, IEnumerable<object?> listValues) {
        var token = lexer.GetNextToken(TokenType.ListClose | TokenType.ListNextItem);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token!.Type == TokenType.ListNextItem) {
            ParseLiteralList(lexer, filter, listValues);
        }
        else if(token!.Type == TokenType.ListClose) {
            ProcessListClose(filter, listValues);
            ParseAndOr(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Unexpected token. Expected a literal.");
        }
    }

    private void ParseAndOr(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.And | TokenType.Or | TokenType.EndOfFile);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token.Type == TokenType.Or) {
            filter.AddClause(false);
            ParsePropertyOrPropertyHeader(lexer, filter);
        }
        else if(token.Type == TokenType.And) {
            filter.AddClause(true);
            ParseProperty(lexer, filter);
        }
        else if(token.Type == TokenType.EndOfFile) {
            return;
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Unexpected token. Expected &&, || or end of file.");
        }
    }

    private void ParsePropertyOrPropertyHeader(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Property | TokenType.PropertyHeader);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token.Type == TokenType.PropertyHeader) {
            ProcessPropertyHeader(lexer, filter, token);
            ParsePropertyOrPropertyHeader(lexer, filter);
        }
        else if(token!.Type == TokenType.Property) {
            ProcessProperty(filter, token);
            ParseOperatorOrListOperator(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Internal error. Bad token returned.");
        }
    }

    private void ParseProperty(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Property);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token!.Type == TokenType.Property) {
            ProcessProperty(filter, token);
            ParseOperatorOrListOperator(lexer, filter);
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char, "Internal error. Bad token returned.");
        }
    }

    private void ProcessListClose(Filter filter, IEnumerable<object?> listValues) {
        filter.LastClause!.Parameter = listValues;
    }

    private void ProcessListLiteral(FilterLexer lexer, Filter filter, Token token) {
        TypeCheckProperty(lexer, filter, token);
    }

    private void ProcessLiteral(FilterLexer lexer, Filter filter, Token token) {
        var condition = filter.LastClause!;
        if (condition.ListOperation != ListOperation.NotAListOperation) {
            throw new FilterParserException(
                "Incompatible multi-operator. Multi-operator can only be applied on list values.");
        }
        TypeCheckProperty(lexer, filter, token);
        condition.Parameter = token.Value;
    }

    private static void TypeCheckProperty(FilterLexer lexer, Filter filter, Token token) {
        var clause = filter.LastClause!;

        if (token.Type is TokenType.NullLiteral &&
            clause.Operation is not Operation.Equals and not Operation.NotEquals) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                $"Incompatible operator. Null literal only supports equality.");
        }

        var propertyName = clause.Property;
        var property = typeof(ItemOwnedModel).GetProperty(propertyName)!;
        var type = property.PropertyType;

        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
            // Special case for Count in List properties
            if (token.Type is TokenType.IntLiteral && clause.Operation is Operation.CountEquals
                    or Operation.CountNotEquals or Operation.CountLessThan
                    or Operation.CountLessThanOrEqual or Operation.CountGreaterThan
                    or Operation.CountGreaterThanOrEqual) {
                return;
            }

            type = type.GetGenericArguments()[0];
        }
        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            type = type.GetGenericArguments()[0];
        }

        // Special case for item styles
        if (token.Type == TokenType.StringLiteral && (type == typeof(ItemStyleRaw) || type == typeof(ItemStyleModel))) {
            return;
        }

        if(token.Value != null && token.Value.GetType() != type) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                $"Incompatible data types. Can't compare parameter of type {token.Value.GetType()} with {propertyName}");
        }
    }

    private void ProcessListOpen(FilterLexer lexer, Filter filter) {
        var condition = filter.LastClause!;
        if (!strict && condition.ListOperation == ListOperation.NotAListOperation) {
            condition.ListOperation = ListOperation.Any;
        }
        else if(condition.ListOperation == ListOperation.NotAListOperation) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                "Invalid list usage. Tried to use a list without list operator.");
        }
    }

    private void ProcessListOperator(Filter filter, Token token) {
        filter.LastClause!.ListOperation = (ListOperation) token.Value!;
    }

    private void ProcessOperator(FilterLexer lexer, Filter filter, Token token) {
        var op = (Operation) token.Value!;
        filter.LastClause!.Operation = op;


        var propertyName = filter.LastClause!.Property;
        var property = typeof(ItemOwnedModel).GetProperty(propertyName)!;
        var type = property.PropertyType;

        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            type = type.GetGenericArguments()[0];
        }

        bool supported;
        if(type == typeof(string)) {
            supported = op is Operation.Equals or Operation.NotEquals or Operation.GreaterThan
                or Operation.GreaterThanOrEqual or Operation.LessThan or Operation.LessThanOrEqual
                or Operation.StrictEquals or Operation.StrictNotEquals or Operation.Contains
                or Operation.NotContains;
        }
        else if(type == typeof(decimal)) {
            supported = op is Operation.Equals or Operation.NotEquals or Operation.GreaterThan
                or Operation.GreaterThanOrEqual or Operation.LessThan or Operation.LessThanOrEqual
                or Operation.StrictEquals or Operation.StrictNotEquals;
        }
        else if(type == typeof(int) || type == typeof(DateTime) ||
                type == typeof(ItemSource) || type == typeof(ItemRarity) || type == typeof(ItemType)) {
            supported = op is Operation.Equals or Operation.NotEquals or Operation.GreaterThan
                or Operation.GreaterThanOrEqual or Operation.LessThan or Operation.LessThanOrEqual;
        }
        else if(type == typeof(IEnumerable<string>) || type == typeof(IEnumerable<ItemTag>) || 
                type == typeof(IEnumerable<ItemStyleRaw>) || type == typeof(IEnumerable<ItemStyleModel>)) {
            supported = op is Operation.Contains or Operation.NotContains or Operation.CountEquals or Operation.CountNotEquals or Operation.CountGreaterThan
                or Operation.CountGreaterThanOrEqual or Operation.CountLessThan or Operation.CountLessThanOrEqual;
        }
        else {
            throw new FilterParserException(lexer.Line, lexer.Char,
                "Internal error. Tried to access property with invalid type.");
        }

        if(!supported) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                $"Incompatible operator. Can't apply {Enum.GetName(op)} on {propertyName}.");
        }
    }

    private void ProcessProperty(Filter filter, Token token) {
        filter.LastClause!.Property = (string) token.Value!;
    }

    private void ProcessHeader(FilterLexer lexer, Filter filter, Token token) {
        var header = (HeaderInformation) token.Value!;
        switch(header.Type) {
            case HeaderType.Version when uint.TryParse(header.Value, out var intVersion):
                filter.Version = (int) intVersion;
                break;
            case HeaderType.Version:
                throw new FilterParserException("Version number can only be a positive number.");
            case HeaderType.Name:
                filter.Name = header.Value;
                break;
            case HeaderType.Description:
                filter.Description = header.Value;
                break;
            case HeaderType.Author:
                filter.Author = header.Value;
                break;
            case HeaderType.Optimization when Enum.TryParse("L" + header.Value, out OptimizationLevel level):
                filter.OptimizationLevel = level;
                break;
            case HeaderType.Optimization:
                throw new FilterParserException("Invalid optimization level. Valid values are 0/1/2.");
            case HeaderType.Strict when bool.TryParse(header.Value, out bool value):
                strict = value;
                lexer.Strict = value;
                break;
            case HeaderType.Strict:
                throw new FilterParserException("Invalid strict header value. Valid values are true/false.");
            case HeaderType.AllPass when bool.TryParse(header.Value, out bool value):
                filter.AllPass = value;
                break;
            case HeaderType.AllPass:
                throw new FilterParserException("Invalid all pass header value. Valid values are true/false.");
        }
    }

    private void ProcessPropertyHeader(FilterLexer lexer, Filter filter, Token token) {
        var header = (PropertyHeaderInformation) token.Value!;
        switch(header.Type) {
            case PropertyHeaderType.Remark:
                filter.LastClause!.Remark = (string) header.Value;
                break;
        }
    }
}