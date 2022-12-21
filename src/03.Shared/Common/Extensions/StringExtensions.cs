﻿using System.Text;
using System.Text.RegularExpressions;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Shared.Common.Extensions;

public static class StringExtensions
{
    public static string SplitWords(this string sentence)
    {
        var pattern = @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])";
        var regex = new Regex(pattern, RegexOptions.Compiled);
        var removedExistingSpace = sentence.Replace(" ", string.Empty);

        return regex.Replace(removedExistingSpace, " ");
    }

    public static string ReplaceNewLineToBr(this string text)
    {
        var pattern = @"(\r\n|\r|\n)+";
        var regex = new Regex(pattern, RegexOptions.Compiled);

        return regex.Replace(text, "<br />");
    }

    public static string Replace(this string source, IDictionary<string, string> replacements)
    {
        var result = new StringBuilder(source);

        foreach (var replacement in replacements)
        {
            result.Replace(replacement.Key, replacement.Value);
        }

        return result.ToString();
    }

    public static string ToSafeDisplayText(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return DefaultTextFor.Dash;
        }

        return text;
    }
}
