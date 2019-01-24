using System;

namespace TMDbLibTests.Exceptions
{
    public class ConfigurationErrorsException : Exception
    {
        public ConfigurationErrorsException(string message) : base(message)
        {
        }
    }
}
