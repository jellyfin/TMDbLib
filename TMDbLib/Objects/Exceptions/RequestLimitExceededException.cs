using System;

namespace TMDbLib.Objects.Exceptions
{
    public class RequestLimitExceededException : Exception
    {
        public DateTimeOffset? RetryOn { get; }

        public TimeSpan? RetryAfter { get; }

        internal RequestLimitExceededException(DateTimeOffset? retryOn, TimeSpan? retryAfter)
            : base("You have exceeded the maximum number of request allowed by TMDb please try again later")
        {
            RetryOn = retryOn;
            RetryAfter = retryAfter;
        }
    }
}