using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Application.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
        {
            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }

        public static bool IsValidJson(this string jsonString)
        {
            try
            {
                JsonSerializer.Deserialize<dynamic>(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int[] ToIntArray(this string source, string separator = ",")
        {
            return source?.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c =>
                {
                    if (int.TryParse(c, out int _value))
                        return _value;
                    return default(int?);
                }).Where(c => c.HasValue)
                .Select(c => c.Value)
                .ToArray() ?? new int[0];
        }

        public static int?[] ToNullableIntArray(this string source, string separator = ",")
        {
            return source?.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c =>
                {
                    if (int.TryParse(c, out int _value))
                        return _value;
                    return default(int?);
                }).Where(c => c.HasValue)
                .Select(c => (int?)c.Value)
                .ToArray() ?? new int?[0];
        }

        public static bool EqualsIgnoreCase(this string str, string value)
        {
            return string.Equals(str, value, StringComparison.OrdinalIgnoreCase);
        }

        public static string ConvertNullToEmptyString(this string value)
        {
            return value is null ? string.Empty : value;
        }

        public static string RemoveHtmlTag(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}