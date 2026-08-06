using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Authentication
{
    /// <summary>
    /// Specifies the type of session used for authentication.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter<SessionType>))]
    public enum SessionType
    {
        /// <summary>
        /// No session type assigned.
        /// </summary>
        Unassigned = 0,

        /// <summary>
        /// A guest session that doesn't require user authentication.
        /// </summary>
        GuestSession = 1,

        /// <summary>
        /// A user session with full authentication.
        /// </summary>
        UserSession = 2
    }
}
