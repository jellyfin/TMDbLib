using System;
using System.Net;

namespace TMDbLib.Objects.Exceptions
{
    public class RequestLimitExceededException : TMDbHttpException
    {
        public DateTimeOffset? RetryOn { get; }

        public TimeSpan? RetryAfter { get; }

        internal RequestLimitExceededException(HttpStatusCode statusCode, TMDbStatusMessage statusMessage, DateTimeOffset? retryOn, TimeSpan? retryAfter)
            : base("You have exceeded the maximum number of request allowed by TMDb please try again later", statusCode, statusMessage)
        {
            RetryOn = retryOn;
            RetryAfter = retryAfter;
        }
    }
}