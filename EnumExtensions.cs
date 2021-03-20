using Microsoft.Extensions.Localization;
using System;
using System.Linq;

namespace Energetic.Text
{
    public static class EnumExtensions
    {
        private static PluralizerAttribute GetQuantifierAttribute<TEnum>(this TEnum instance)
            where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var memberInfos = enumType.GetMember(instance.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var attributes = enumValueMemberInfo.GetCustomAttributes(typeof(PluralizerAttribute), false);
            return ((PluralizerAttribute)attributes[0]);
        }

        public static string GetSingularTerm<TEnum>(this TEnum instance, IStringLocalizer? localizer = null)
            where TEnum : Enum
        {
            return instance.GetQuantifierAttribute().Singular;
        }

        public static string GetPluralTerm<TEnum>(this TEnum instance, IStringLocalizer? localizer = null)
            where TEnum : Enum
        {
            return instance.GetQuantifierAttribute().Plural;
        }

        public static string GetDualTerm<TEnum>(this TEnum instance, IStringLocalizer? localizer = null)
            where TEnum : Enum
        {
            return instance.GetQuantifierAttribute().Dual;
        }

        public static string GetAppropriateSingularOrPluralTerm<TEnum>(this TEnum instance,
            long quantity,
            IStringLocalizer? localizer = null)
            where TEnum : Enum
        {
            switch (quantity)
            {
                case 1:
                    return instance.GetSingularTerm(localizer);
                case -1:
                    return instance.GetSingularTerm(localizer);
                case 2:
                    return instance.GetDualTerm(localizer);
                default:
                    return instance.GetPluralTerm(localizer);
            }
        }
    }
}
