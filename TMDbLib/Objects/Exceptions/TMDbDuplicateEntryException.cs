namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Thrown when TMDb rejects a mutation because the data already exists (status code 8).
/// </summary>
public class TMDbDuplicateEntryException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbDuplicateEntryException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    public TMDbDuplicateEntryException(TMDbStatusMessage? statusMessage)
        : base(statusMessage?.StatusMessage ?? "The data you tried to submit already exists.", statusMessage)
    {
    }
}
