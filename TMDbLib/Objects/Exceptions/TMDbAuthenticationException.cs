namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Thrown when TMDb rejects a request due to authentication or authorization issues
/// (invalid/suspended API key, denied session, missing permissions, expired tokens, etc.).
/// </summary>
public class TMDbAuthenticationException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbAuthenticationException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    public TMDbAuthenticationException(TMDbStatusMessage? statusMessage)
        : base(statusMessage?.StatusMessage ?? "TMDb authentication failed.", statusMessage)
    {
    }
}
