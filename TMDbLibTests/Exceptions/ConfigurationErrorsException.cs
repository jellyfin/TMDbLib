using System;

namespace TMDbLibTests.Exceptions;

/// <summary>
/// Exception thrown when test configuration is invalid or missing.
/// </summary>
public class ConfigurationErrorsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationErrorsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the configuration error.</param>
    public ConfigurationErrorsException(string message) : base(message)
    {

    }
}
