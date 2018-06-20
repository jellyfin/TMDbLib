using System.Net;

namespace TMDbLib.Objects.Exceptions
{
    public class TMDbHttpException : TMDbException
    {
        public HttpStatusCode HttpStatusCode { get; }

        public TMDbHttpException(string message, HttpStatusCode httpStatusCode, TMDbStatusMessage statusMessage)
                       : base(message, statusMessage)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}