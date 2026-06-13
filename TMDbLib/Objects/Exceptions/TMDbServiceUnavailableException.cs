namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Thrown when TMDb is temporarily unavailable (service offline, maintenance, backend unreachable, backend timeout).
/// </summary>
public class TMDbServiceUnavailableException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbServiceUnavailableException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    public TMDbServiceUnavailableException(TMDbStatusMessage? statusMessage)
        : base(statusMessage?.StatusMessage ?? "TMDb service is currently unavailable.", statusMessage)
    {
    }
}
