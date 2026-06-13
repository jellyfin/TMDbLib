namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Thrown when TMDb reports an internal server-side error (HTTP 5xx that isn't a known recoverable state).
/// </summary>
public class TMDbServerException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbServerException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    public TMDbServerException(TMDbStatusMessage? statusMessage)
        : base(statusMessage?.StatusMessage ?? "TMDb returned an internal server error.", statusMessage)
    {
    }
}
