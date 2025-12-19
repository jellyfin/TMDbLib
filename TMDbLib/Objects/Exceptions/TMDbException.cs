using System;

namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Represents the base exception class for all TMDb-related exceptions.
/// </summary>
public class TMDbException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TMDbException(string message)
            : base(message)
    {
    }
}
