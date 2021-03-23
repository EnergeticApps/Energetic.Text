using Microsoft.Extensions.Localization;
using Energetic.Text;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Minimizes whitespace in the string by replacing all multiple occurences of whitespace with a single space character.
        /// </summary>
        /// <param name="value"></param>
        /// <example>Input: "Hi   you"; Output: "Hi you"</example>
        /// <example>Input: "Foo
        /// Bar"; Output: "Foo Bar"</example>
        /// <remarks>Works on newline characters, tabs and all forms of whitespace.</remarks>
        public static string CollapseWhiteSpace(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Minimizes whitespace in the string by replacing all multiple occurences of the space character with a single space character.
        /// </summary>
        /// <param name="value"></param>
        /// <example>Input: "Hi   you"; Output: "Hi you"</example>
        /// <remarks>Only operates on space characters, not newline characters, tabs or any other forms of whitespace.</remarks>
        public static string CollapseMultipleSpaces(this string value)
        {
            return Regex.Replace(value, " +", " ");
        }


        /// <summary>
        /// A fallback option in case more advanced pluralizations don't yield a result, this function just naively adds an 's'
        /// on the end of any string that doesn't end in an 's' already, or an 'es' on the end of any string that already
        /// ends in an 's'.
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>Although this only works with English regular nouns. To pluralize irregular English nouns, or nouns in other languages,
        /// consider using a <see cref="PluralizerAttribute" /> instead.</remarks>
        public static string NaivePluralize(this string value)
        {
            return value.ToLowerInvariant().EndsWith('s') ? value + "es" : value + "s";
        }

        public static string Append(this string value, string appendation)
        {
            return value.Append(string.Empty, appendation);
        }

        /// <summary>
        /// Appends the appendation string to the input string, separating them with the separator string. The separator string
        /// is only added if it isn't already present at the end of the input string or the start of the appendation.
        /// </summary>
        /// <param name="value">The input string</param>
        /// <param name="separator">The string to use as a separator, unless it's already present in the input or appendation.</param>
        /// <param name="appendation">The string to append to the input string.</param>
        /// <example>
        /// <code>
        /// string input = @"c:\";
        /// string separator = @"\";
        /// string appendation = @"\users\administrator";
        /// string output = input.AppendWithSeparator(separator, appendation);
        /// // Output is "c:\users\administrator"
        /// // Note that the separator wasn't added because it was already there.
        /// </code>
        /// </example>
        public static string Append(this string value, string appendation, string separator)
        {
            string result = value ?? string.Empty;
            appendation ??= string.Empty;
            separator ??= string.Empty;

            if (string.IsNullOrEmpty(appendation))
                return result;

            if (result.EndsWith(separator) && appendation.StartsWith(separator))
            {
                int takeChars = appendation.Length - separator.Length;
                appendation = appendation.Right(takeChars);
            }

            if (result.EndsWith(separator) || appendation.StartsWith(separator) || result == string.Empty)
            {
                result += appendation;
            }
            else
            {
                result += separator + appendation;
            }

            return result;
        }

        /// <summary>
        /// Removes all the items that are null or empty strings or white space from the enumerable.
        /// </summary>
        /// <param name="value">An enumerable of strings.</param>
        /// <returns>An enumerable resembling the input enumerable but with all the null, empty and white space items removed.</returns>
        public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string?>? enumerable)
        {
            if (enumerable is null)
                return new List<string>();

            return enumerable.Where(item => !string.IsNullOrWhiteSpace(item))!;
        }

        /// <summary>
        /// Creates a comma separated list of items in the input enumerable, and will optionally say "and" between the penultumate
        /// and the final item.
        /// </summary>
        /// <param name="items">An enumerable of strings.</param>
        /// <param name="sayAndBeforeFinalItem">Whether or not to say "and" between the penultumate and the final item.</param>
        public static string ToCommaSeparatedList(this IEnumerable<string?>? items, bool sayAndBeforeFinalItem = false)
        {
            if (items is null)
                return string.Empty;

            items = items.WhereNotNullOrWhiteSpace();

            if (items.Count() == 1)
                return items.First()!;

            var array = items.ToArray();
            var builder = new StringBuilder();
            int first = 0;
            int last = array.Length - 1;
            int penultimate = last - 1;

            for (int i = first; i < penultimate; i++)
            {
                builder.Append($"{array[i]}, ");
            }

            if (sayAndBeforeFinalItem)
            {
                builder.Append($"{array[penultimate]} and ");
            }
            else
            {
                builder.Append($"{array[penultimate]}, ");
            }

            builder.Append($"{array[last]}");

            return builder.ToString();
        }


        /// <summary>
        /// Returns the desired number of characters from the left hand side of a string.
        /// </summary>
        /// <param name="value">The input string</param>
        /// <param name="maxLength">The maximum number of characters to take.</param>
        /// <remarks>Takes less than the maximum number of characters if the input string is shorter than the maximum.</remarks>
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }
            else
            {
                maxLength = Math.Abs(maxLength);

                return value.Substring(0, maxLength);
            }
        }

        /// <summary>
        /// Returns the desired number of characters from the right hand side of a string.
        /// </summary>
        /// <param name="value">The input string</param>
        /// <param name="maxLength">The maximum number of characters to take.</param>
        /// <remarks>Takes less than the maximum number of characters if the input string is shorter than the maximum.</remarks>
        public static string Right(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }
            else
            {
                maxLength = Math.Abs(maxLength);

                return value.Substring(value.Length - maxLength, maxLength);
            }
        }

        /// <summary>
        /// Cuts any characters off the end of a string that extend beyond the maxLength. Can then add on a trailing string if desired.
        /// Used for creating summaries.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="trailing"></param>
        /// <returns></returns>
        /// <remarks>
        ///  For example:
        ///  var description = "This is a long string";
        ///  var summary = description.Truncate(10, "...");
        ///  result:
        ///  "This is a..."
        /// </remarks>
        public static string Truncate(this string value, int maxLength, string trailing = "", bool trimWhiteSpace = true)
        {
            string result = value.Left(maxLength - trailing.Length);

            if (trimWhiteSpace)
                result = result.Trim();

            if (!string.IsNullOrWhiteSpace(trailing))
                result += trailing;

            return result;
        }

        /// <summary>
        /// Trims (i.e. removes leading and trailing whitespace from) every string in an enumerable.
        /// </summary>
        /// <typeparam name="T">A type implementing <see cref="IEnumerable{string}"/> such as an array of strings or a <see cref="List{string}". /></typeparam>
        /// <param name="strings">The enumerable of strings that should each be trimmed.</param>
        /// <returns></returns>
        public static T TrimAll<T>(this T strings)
            where T : IEnumerable<string>
        {
            return (T)strings.Select(str => str.Trim());
        }



        public static string Localize(this string value, IStringLocalizer? localizer)
        {
            return localizer?[value] ?? value;
        }


        public static string Localize(this string value, IStringLocalizer? localizer, params object[] args)
        {
            return localizer?[value, args] ?? string.Format(value, args);
        }

        public static string ToCase(this string value, Capitalization capitalization)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return capitalization switch
            {
                Capitalization.Lowercase => value.ToLowerInvariant(),
                Capitalization.Uppercase => value.ToUpperInvariant(),
                Capitalization.TitleCase => value.ToTitleCase(),
                Capitalization.TitleCaseExceptConjunctions => value.ToTitleCase(true),
                Capitalization.SentenceCase => value.ToSentenceCase(),
                _ => value,
            };
        }

        public static string ToTitleCase(this string value, bool lowerCaseConjunctions = false)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.IsAllSameCase())
                value = value.ToLowerInvariant();

            var result = Regex.Replace(value, @"\b(\w)", m => m.Value.ToUpper());

            if (lowerCaseConjunctions)
                result = Regex.Replace(result, @"(\s(of|in|by|and)|\'[st])\b",
                    m => m.Value.ToLower(), RegexOptions.IgnoreCase);

            return result;
        }

        public static bool IsAllSameCase(this string value)
        {
            return (value == value.ToUpperInvariant() || value == value.ToLowerInvariant());
        }

        public static string ToSentenceCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.ToLowerInvariant();
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            return r.Replace(value, s => s.Value.ToUpper());
        }
    }
}
