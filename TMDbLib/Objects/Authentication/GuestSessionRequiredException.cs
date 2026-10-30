using System;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// Exception thrown when a method requires a guest or user session but none is set.
/// </summary>
public class GuestSessionRequiredException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GuestSessionRequiredException"/> class.
    /// </summary>
    public GuestSessionRequiredException()
        : base("The method you called requires a valid guest or user session to be set on the client object. Please use the 'SetSessionInformation' method to do so.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GuestSessionRequiredException"/> class with a specific message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public GuestSessionRequiredException(string message)
        : base(message)
    {
    }
}
