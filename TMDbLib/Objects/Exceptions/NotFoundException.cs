namespace TMDbLib.Objects.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a requested item is not found in the TMDb API.
    /// </summary>
    public class NotFoundException : APIException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="statusMessage">The TMDb status message.</param>
        public NotFoundException(TMDbStatusMessage? statusMessage)
                        : base("The requested item was not found", statusMessage)
        {
        }
    }
}
