namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Thrown when TMDb rejects a request because it failed validation
/// (invalid parameters, malformed date, page out of range, too many append-to-response items, etc.).
/// </summary>
public class TMDbValidationException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbValidationException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    public TMDbValidationException(TMDbStatusMessage? statusMessage)
        : base(statusMessage?.StatusMessage ?? "TMDb request validation failed.", statusMessage)
    {
    }
}
