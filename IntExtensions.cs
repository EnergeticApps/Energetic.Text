using Microsoft.Extensions.Localization;
using Energetic.Text;
using System;

namespace System
{
    public static class IntExtensions
    {
        public static string GetAppropriateSingularOrPluralTerm(
            this int number,
            string singularTerm,
            string pluralTerm,
            string? dualTerm = null,
            IStringLocalizer? localizer = null)
        {
            dualTerm ??= pluralTerm;

            switch (Math.Abs(number))
            {
                case 1:
                    return singularTerm.Localize(localizer);
                case 2:
                    return dualTerm.Localize(localizer);
                default:
                    return pluralTerm.Localize(localizer);
            }
        }

        public static string ToWords(this int number, bool continueBeyondTen = false)
        {
            if (continueBeyondTen)
                throw new NotImplementedException("This function doesn't work beyond the number 10");

            switch (number)
            {
                case 0:
                    return "zero";
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                case 10:
                    return "ten";
                default:
                    return number.ToString();
            }
        }
    }
}
