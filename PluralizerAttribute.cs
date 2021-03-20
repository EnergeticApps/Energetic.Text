using System;

namespace Energetic.Text
{
    [AttributeUsage(AttributeTargets.All)]
    public class PluralizerAttribute : Attribute
    {
        public PluralizerAttribute(string singular, string plural, string? dual = null)
        {
            Singular = string.IsNullOrWhiteSpace(singular) ? throw new StringArgumentNullOrWhiteSpaceException(nameof(singular)) : singular;
            Plural = string.IsNullOrWhiteSpace(plural) ? throw new StringArgumentNullOrWhiteSpaceException(nameof(plural)) : plural;
            Dual = string.IsNullOrWhiteSpace(dual) ? plural : dual!;
        }

        public string Singular { get; }
        public string Plural { get; }
        public string Dual { get; }
    }
}
