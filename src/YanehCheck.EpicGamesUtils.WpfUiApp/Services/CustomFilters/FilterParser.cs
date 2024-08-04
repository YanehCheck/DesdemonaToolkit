using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;

/// <inheritdoc cref="IFilterParser"/>
public class FilterParser : IFilterParser {
    public IFilter Parse(string filterString) {
        var lexer = new FilterLexer(filterString);
        var filter = new Filter();
        ParseHeaderOrProperty(lexer, filter);
        return filter;
    }

    private void ParseHeaderOrProperty(FilterLexer lexer, Filter filter) {
        var token = lexer.GetNextToken(TokenType.Header | TokenType.Property | TokenType.EndOfFile);
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(token.Type == TokenType.Header) {
            ProcessHeader(filter, token);
            ParseHeaderOrProperty(lexer, filter);
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
        var token = lexer.GetNextToken(TokenTypeExtensions.AnyLiteral());
        if(token is null) {
            throw new FilterParserException(lexer.Line, lexer.Char, "Syntax error. Unknown or unexpected token.");
        }
        else if(TokenTypeExtensions.IsLiteral(token.Type)) {
            ProcessListLiteral(lexer, filter, token);
            listValues = listValues.Append(token.Value);
            ParseListCloseOrListNext(lexer, filter, listValues);
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
            ParseProperty(lexer, filter);
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
        TypeCheckProperty(lexer, filter, token);
        filter.LastClause!.Parameter = token.Value;
    }

    private static void TypeCheckProperty(FilterLexer lexer, Filter filter, Token token) {
        var clause = filter.LastClause!;

        if (token.Type is TokenType.NullLiteral &&
            clause.Operation is not Operation.Equals and not Operation.NotEquals) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                $"Incompatible operator. Null literal only supports equality.");
        }

        var propertyName = clause.Property;
        var property = typeof(ItemModel).GetProperty(propertyName)!;
        var type = property.PropertyType;

        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
            type = type.GetGenericArguments()[0];
        }
        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            type = type.GetGenericArguments()[0];
        }

        if(token.Value != null && token.Value.GetType() != type) {
            throw new FilterParserException(lexer.Line, lexer.Char,
                $"Incompatible data types. Can't compare parameter of type {token.Value.GetType()} with {propertyName}");
        }
    }

    private void ProcessListOpen(FilterLexer lexer, Filter filter) {
        if(filter.LastClause!.ListOperation == ListOperation.NotAListOperation) {
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
        var property = typeof(ItemModel).GetProperty(propertyName)!;
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
                type == typeof(ItemSource) || type == typeof(ItemRarity)) {
            supported = op is Operation.Equals or Operation.NotEquals or Operation.GreaterThan
                or Operation.GreaterThanOrEqual or Operation.LessThan or Operation.LessThanOrEqual;
        }
        else if(type == typeof(IEnumerable<string>) || type == typeof(IEnumerable<ItemTag>)) {
            supported = op is Operation.Contains or Operation.NotContains;
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


    private void ProcessHeader(Filter filter, Token token) {
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
        }
    }

}