using System;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// Exception thrown when a method requires a user session but none is set.
/// </summary>
public class UserSessionRequiredException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserSessionRequiredException"/> class.
    /// </summary>
    public UserSessionRequiredException()
        : base("The method you called requires a valid user session to be set on the client object. Please use the 'SetSessionInformation' method to do so.")
    {
    }
}
