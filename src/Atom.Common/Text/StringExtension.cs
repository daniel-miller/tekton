using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Common
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string s) 
            => s == null || s.Length == 0 || string.IsNullOrWhiteSpace(s);

        public static bool IsNotEmpty(this string s) 
            => !IsEmpty(s);

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
        public static bool MatchesAny(this string value, string[] others)
        {
            foreach (var other in others)
                if (Matches(value, other))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if a string has zero matches in an array of other strings.
        /// </summary>
        public static bool MatchesNone(this string value, string[] others)
        {
            return !MatchesAny(value, others);
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
    }
}