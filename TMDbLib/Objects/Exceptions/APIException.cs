namespace TMDbLib.Objects.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when the TMDb API returns an error.
    /// </summary>
    public class APIException : TMDbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusMessage">The TMDb status message.</param>
        public APIException(string message, TMDbStatusMessage? statusMessage) : base(message)
        {
            StatusMessage = statusMessage;
        }

        /// <summary>
        /// Gets the status message returned by the TMDb API.
        /// </summary>
        public TMDbStatusMessage? StatusMessage { get; }
    }
}
