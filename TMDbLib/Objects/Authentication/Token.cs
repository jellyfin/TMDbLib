using System;

namespace TMDbLib.Objects.Authentication
{
    /// <summary>
    /// A request token is required in order to request a user authenticated session id.
    /// Request tokens will expire after 60 minutes. 
    /// As soon as a valid session id has been created the token will be useless.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The date / time before which the token must be used, else it will expire. Time is expressed as local time.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        public string RequestToken { get; set; }
        public bool Success { get; set; }
        public string AuthenticationCallback { get; set; }
    }
}
