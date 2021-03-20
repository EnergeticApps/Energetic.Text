using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energetic.Text
{
    public class StringArgumentNullOrWhiteSpaceException : ArgumentException
    {
        public StringArgumentNullOrWhiteSpaceException(string paramName) : base("String cannot be null, empty or just white space.", paramName)
        {

        }
    }
}
