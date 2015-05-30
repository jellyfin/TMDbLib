using System;

namespace TMDbLib.Objects.Exceptions
{
    public class RequestLimitExceededException : Exception
    {
        public RequestLimitExceededException()
            : base("You have exceeded the maximum number of request allowed by TMDb please try again later")
        {

        }
    }
}