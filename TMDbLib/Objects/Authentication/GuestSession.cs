using System;

namespace TMDbLib.Objects.Authentication
{
    /// <summary>
    /// A guest session can be used to rate movies/tv shows without having a registered TMDb user account. 
    /// You should only generate a single guest session per user (or device) as you will be able to attach the ratings to a TMDb user account in the future. 
    /// There is also IP limits in place so you should always make sure it's the end user doing the guest session actions.
    /// If a guest session is not used for the first time within 24 hours, it will be automatically discarded.
    /// </summary>
    public class GuestSession
    {
        public string GuestSessionId { get; set; }
        public bool Success { get; set; }
        /// <summary>
        /// The date / time before which the session must be used for the first time else it will expire. Time is expressed as local time.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
