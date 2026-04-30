using System.Text.RegularExpressions;

namespace Infrastructure;

public static partial class InputSanitizer
{
    [GeneratedRegex("<[^>]+>", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
    
    private static readonly Regex HtmlRegex = MyRegex();
    
    public static string SanitizePlainText(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var trim = input.Trim();

        return HtmlRegex.IsMatch(input) ? throw new ArgumentException("HTML is not allowed in input") : trim;
    }

    
}