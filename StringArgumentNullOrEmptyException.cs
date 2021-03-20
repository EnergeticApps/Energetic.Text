using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energetic.Text
{
    public class EnumerableEmptyException : ArgumentException
    {
        public EnumerableEmptyException(string paramName) : base("String cannot be null or empty.", paramName)
        {
        }
    }
}
