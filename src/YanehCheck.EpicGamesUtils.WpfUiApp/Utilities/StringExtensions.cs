using System.Globalization;
using System.Text;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities;

public static class StringExtensions {
    public static string RemoveAccents(this string text) {
        if(string.IsNullOrWhiteSpace(text)) {
            return text;
        }

        text = text.Normalize(NormalizationForm.FormD);
        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}