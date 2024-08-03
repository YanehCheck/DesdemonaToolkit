using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;

/// <summary>
/// Parses a filter from a string. Represents syntax and semantic analysis.
/// </summary>
/// <exception cref="FilterParserException">When source code is invalid.</exception>
public interface IFilterParser
{
    IFilter Parse(string filterString);
}