using System.Text.RegularExpressions;

namespace PlasterSkull.Blazor
{
    public static partial class StringExtensions
    {
        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return KebabCaseRegex().Replace(value, "-$1")
                .Trim()
                .ToLower();
        }

        [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled)]
        private static partial Regex KebabCaseRegex();
    }
}
