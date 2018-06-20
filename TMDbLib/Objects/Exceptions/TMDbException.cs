using System;

namespace TMDbLib.Objects.Exceptions
{
    public class TMDbException : Exception
    {
        public TMDbStatusMessage StatusMessage { get; }

        public TMDbException(string message, TMDbStatusMessage statusMessage)
                : base(message)
        {
            StatusMessage = statusMessage;
        }
    }
}