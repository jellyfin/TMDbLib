using System;

namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Represents an exception that is thrown when the TMDb API request limit has been exceeded.
/// </summary>
public class RequestLimitExceededException : APIException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLimitExceededException"/> class.
    /// </summary>
    /// <param name="statusMessage">The TMDb status message.</param>
    /// <param name="retryOn">The date and time when requests can be retried.</param>
    /// <param name="retryAfter">The time span after which requests can be retried.</param>
    internal RequestLimitExceededException(TMDbStatusMessage? statusMessage, DateTimeOffset? retryOn, TimeSpan? retryAfter)
        : base("You have exceeded the maximum number of request allowed by TMDb please try again later", statusMessage)
    {
        RetryOn = retryOn;
        RetryAfter = retryAfter;
    }

    /// <summary>
    /// Gets the date and time when requests can be retried.
    /// </summary>
    public DateTimeOffset? RetryOn { get; }

    /// <summary>
    /// Gets the time span after which requests can be retried.
    /// </summary>
    public TimeSpan? RetryAfter { get; }
}
