using System.Net;

namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Represents an exception that occurs when TMDb returns an unexpected HTTP error.
/// </summary>
public class GeneralHttpException : TMDbException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralHttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    public GeneralHttpException(HttpStatusCode httpStatusCode)
        : base($"TMDb returned an unexpected HTTP error: {(int)httpStatusCode}")
    {
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Gets the HTTP status code returned by TMDb.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }
}
