using Microsoft.Extensions.Localization;
using Energetic.Text;
using System;

namespace System
{
    public static class TypeExtensions
    {
        public static string GetSingularTerm(this Type type, IStringLocalizer? localizer = null)
        {
            return type.GetAttribute<PluralizerAttribute>(false)?.Singular.Localize(localizer) ?? type.Name;
        }

        public static string GetPluralTerm(this Type type, IStringLocalizer? localizer = null)
        {
            return type.GetAttribute<PluralizerAttribute>(false)?.Plural.Localize(localizer) ?? type.Name.NaivePluralize();
        }

        public static string GetDualTerm(this Type type, IStringLocalizer? localizer = null)
        {
            return type.GetAttribute<PluralizerAttribute>(false)?.Dual.Localize(localizer) ?? type.Name.NaivePluralize();
        }

        public static string GetAppropriateSingularOrPluralTerm(this Type type, long quantity, IStringLocalizer? localizer = null)
        {
            return Math.Abs(quantity) switch
            {
                1 => type.GetSingularTerm(localizer),
                2 => type.GetDualTerm(localizer),
                _ => type.GetPluralTerm(localizer),
            };
        }
    }
}
