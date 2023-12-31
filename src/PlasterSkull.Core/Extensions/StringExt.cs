﻿using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlasterSkull.Core;

public static partial class StringExt
{
    public static string ToKebabCase(this string value) =>
        value.IsNullOrEmpty()
            ? value
            : KebabCaseRegex()
                .Replace(value, "-$1")
                .Trim()
                .ToLower();

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex KebabCaseRegex();

    public static string RemoveMultipleSpaces(string input) =>
        Regex.Replace(input, @"\s+", " ").Trim();

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? source) =>
        string.IsNullOrEmpty(source);
}
