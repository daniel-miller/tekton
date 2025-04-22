using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tek.Base
{
    public static class StringExtension
    {
        public static string Base64Encode(this string s)
        {
            if (s == null)
                return null;

            var bytes = Encoding.UTF8.GetBytes(s);

            return Convert.ToBase64String(bytes);
        }

        public static bool IsEmpty(this string s) 
            => s == null || s.Length == 0 || string.IsNullOrWhiteSpace(s);

        public static bool IsNotEmpty(this string s) 
            => !IsEmpty(s);

        public static string NullIfEmpty(this string value)
            => string.IsNullOrEmpty(value) ? null : value;

        /// <summary>
        /// Returns true if a string matches another string.
        /// </summary>
        /// <remarks>
        /// This is not case-sensitive, two null values match, and any two whitespace values match.
        /// </remarks>
        public static bool Matches(this string value, string other)
        {
            if (value == null && other == null)
                return true;

            if ((value == null && other != null) || (value != null && other == null))
                return false;

            return string.Compare(other, value, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Returns true if a string has one or more matches in an array of other strings.
        /// </summary>
        public static bool MatchesAny(this string value, IEnumerable<string> others)
        {
            if (value.IsEmpty())
                return false;

            foreach (var other in others)
                if (Matches(value, other))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if a string has zero matches in an array of other strings.
        /// </summary>
        public static bool MatchesNone(this string value, IEnumerable<string> others)
        {
            return !MatchesAny(value, others);
        }

        /// <summary>
        /// Returns true if a string starts with any item in an array of strings.
        /// </summary>
        public static bool StartsWithAny(this string value, IEnumerable<string> others)
        {
            if (value.IsEmpty())
                return false;

            foreach (var other in others)
                if (!other.IsEmpty() && value.StartsWith(other, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        /// Converts a string value into a non-null collection.
        /// </summary>
        /// <remarks>
        /// Commas and newlines are assumed to be the delimiters. Empty items are removed from the
        /// collection. Leading and trailing whitespace characters are removed from each item in the
        /// collection.
        /// </remarks>
        public static IEnumerable<string> Parse(this string csv)
        {
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(csv))
                return list;

            list = csv
                .Split(new char[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            return list;
        }

        /// <summary>
        /// Returns a string that contains only letters, digits, underscores, periods, and hyphens.
        /// </summary>
        /// <remarks>
        /// Characters outside the permitted set of characters are replaced with hyphens. This 
        /// return value is web-url-friendly and file-system-friendly.
        /// </remarks>
        public static string Sanitize(this string input)
        {
            if (input.IsEmpty())
                return null;

            // Regular expression to match allowed characters (letters, digits, underscores, hyphens).
            string pattern = "[^a-zA-Z0-9_.-]";

            // Replace disallowed characters with a hyphen.
            string sanitized = Regex.Replace(input, pattern, "-");

            return sanitized;
        }

        /// <summary>
        /// Converts a string value from title case to kebab case.
        /// </summary>
        /// <example>
        /// ToKebabCase("ThisIsAnExample") => "this-is-an-example"
        /// </example>
        public static string ToKebabCase(this string input, bool allowForwardSlash = false)
        {
            string output = null;

            if (input.IsEmpty())
                return output;
            
            if (!allowForwardSlash)
            {
                // Find the boundaries between uppercase letters and other letters.
                output = Regex.Replace(input, "([a-z])([A-Z])", "$1-$2");
            }
            else
            {
                var segments = input.Split(new char[] { '/' });
                
                foreach (var segment in segments)
                {
                    // Find the boundaries between uppercase letters and other letters.
                    output += Regex.Replace(segment, "([a-z])([A-Z])", "$1-$2");

                    if (segment != segments.Last())
                        output += "/";
                }
            }

            // Convert the entire string to lowercase.
            return output.ToLower();
        }
    }
}