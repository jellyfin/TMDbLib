using System;

namespace TMDbLibTests.Exceptions
{
    public class TestFrameworkException : Exception
    {
        public TestFrameworkException(string message) : base(message)
        {
        }
    }
}