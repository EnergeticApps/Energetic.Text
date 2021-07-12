using System;

namespace Energetic.Text
{
    public class StringArgumentNullOrEmptyException : ArgumentException
    {
        public StringArgumentNullOrEmptyException(string paramName) : base("String cannot be null or empty.", paramName)
        {
        }
    }
}