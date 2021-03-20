using System.ComponentModel;

namespace Energetic.Text
{
    public enum Capitalization
    {
        [Description("All letters lowercase.")]
        Lowercase = 1,

        [Description("All letters uppercase.")]
        Uppercase = 2,

        [Description("The first letter of every word uppercase; all other letters lowercase.")]
        TitleCase = 3,

        [Description("The first letter of every word uppercase, except for conjunctions," +
                               "which will remain lowercase; all other letters lowercase.")]
        TitleCaseExceptConjunctions = 4,

        [Description("The first letter of every sentence uppercase. All other letters lowercase.")]
        SentenceCase = 5
    }
}
