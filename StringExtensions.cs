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
        public static string CollapseWhiteSpace(this string instance)
        {
            return Regex.Replace(instance, @"\s+", " ");
        }
        public static string CollapseMultipleSpaces(this string instance)
        {
            return Regex.Replace(instance, " +", " ");
        }

        public static string AppendIfRequired(this string instance, string appendation)
        {
            return instance.AppendIfRequired(string.Empty, appendation);
        }

        /// <summary>
        /// A fallback option in case more advanced pluralizations don't yield a result, this function just naively adds an 's'
        /// on the end of any string that doesn't end in an 's' already, or an 'es' on the end of any string that already
        /// ends in an 's'.
        /// </summary>
        /// <param name="instance"></param>
        /// <remarks>Although this only works with English regular nouns. To pluralize irregular English nouns, or nouns in other languages,
        /// consider using a <see cref="PluralizerAttribute" /> instead.</remarks>
        public static string NaivePluralize(this string instance)
        {
            return instance.ToLowerInvariant().EndsWith('s') ? instance + "es" : instance + "s";
        }

        public static string AppendIfRequired(this string instance, string separator, string appendation)
        {
            string result = instance ?? string.Empty;
            appendation ??= string.Empty;
            separator ??= string.Empty;

            if (appendation != string.Empty)
            {
                if (result.EndsWith(separator) || appendation.StartsWith(separator) || result == string.Empty)
                {
                    result += appendation;
                }
                else
                {
                    result += separator + appendation;
                }
            }

            return result;
        }

        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string?>? enumerable)
        {
            if (enumerable is null)
                return new List<string>();

            return enumerable.Where(item => !string.IsNullOrEmpty(item))!;
        }

        public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string?>? enumerable)
        {
            if (enumerable is null)
                return new List<string>();

            return enumerable.Where(item => !string.IsNullOrWhiteSpace(item))!;
        }


        public static string ToCommaSeparatedList(this IEnumerable<string?>? items, bool sayAndBeforeFinalItem = false)
        {
            if (items is null)
                return string.Empty;

            items = items.WhereNotNullOrWhiteSpace();

            if (items.Count() == 1)
                return items.First();

            var array = items.ToArray();
            var builder = new StringBuilder();
            int first = 0;
            int last = array.Length - 1;
            int penultimate = last - 1;

            for (int i = first; i < penultimate; i ++)
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


        public static string PrependIfRequired(this string value, string prependation, string separator = "")
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(prependation))
                return value;

            if (value.EndsWith(separator) || prependation.StartsWith(separator) || value == string.Empty)
            {
                value = prependation + value;
            }
            else
            {
                value = prependation + separator + value;
            }

            return value;
        }


        public static string Left(this string instance, int maxLength)
        {
            if (string.IsNullOrEmpty(instance) || instance.Length <= maxLength)
            {
                return instance;
            }
            else
            {
                maxLength = Math.Abs(maxLength);

                return instance.Substring(0, maxLength);
            }
        }

        public static string Right(this string instance, int maxLength)
        {
            if (string.IsNullOrEmpty(instance) || instance.Length <= maxLength)
            {
                return instance;
            }
            else
            {
                maxLength = Math.Abs(maxLength);

                return instance.Substring(instance.Length - maxLength, maxLength);
            }
        }

        /// <summary>
        /// Cuts any characters off the end of a string that extend beyond the maxLength. Can then add on a trailing string if desired.
        /// Used for creating summaries.
        /// </summary>
        /// <param name="instance"></param>
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
        public static string Truncate(this string instance, int maxLength, string trailing = "", bool trimWhiteSpace = true)
        {
            string result = instance.Left(maxLength - trailing.Length);

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



        public static string Localize(this string instance, IStringLocalizer? localizer)
        {
            return localizer?[instance] ?? instance;
        }


        public static string Localize(this string instance, IStringLocalizer? localizer, params object[] args)
        {
            return localizer?[instance, args] ?? string.Format(instance, args);
        }

        public static string ToCase(this string instance, Capitalization capitalization)
        {
            if (string.IsNullOrEmpty(instance))
                return string.Empty;

            return capitalization switch
            {
                Capitalization.Lowercase => instance.ToLowerInvariant(),
                Capitalization.Uppercase => instance.ToUpperInvariant(),
                Capitalization.TitleCase => instance.ToTitleCase(),
                Capitalization.TitleCaseExceptConjunctions => instance.ToTitleCase(true),
                Capitalization.SentenceCase => instance.ToSentenceCase(),
                _ => instance,
            };
        }

        public static string ToTitleCase(this string instance, bool lowerCaseConjunctions = false)
        {
            if (string.IsNullOrEmpty(instance))
                return string.Empty;

            if (instance.IsAllSameCase())
                instance = instance.ToLowerInvariant();

            var result = Regex.Replace(instance, @"\b(\w)", m => m.Value.ToUpper());

            if (lowerCaseConjunctions)
                result = Regex.Replace(result, @"(\s(of|in|by|and)|\'[st])\b",
                    m => m.Value.ToLower(), RegexOptions.IgnoreCase);

            return result;
        }

        public static bool IsAllSameCase(this string instance)
        {
            return (instance == instance.ToUpperInvariant() || instance == instance.ToLowerInvariant());
        }

        public static string ToSentenceCase(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
                return string.Empty;

            if (instance.IsAllSameCase())
                instance = instance.ToLowerInvariant();

            return Regex.Replace(instance, @"(?<=(^|[.;:])\s*)[a-z]",
                (match) => match.Value.ToUpper());
        }
    }
}
