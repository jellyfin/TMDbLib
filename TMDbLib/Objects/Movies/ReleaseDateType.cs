using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Movies
{
    /// <summary>
    /// Movie release type.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter<ReleaseDateType>))]
    public enum ReleaseDateType
    {
        /// <summary>
        /// Premiere release.
        /// </summary>
        Premiere = 1,

        /// <summary>
        /// Theatrical limited release.
        /// </summary>
        TheatricalLimited = 2,

        /// <summary>
        /// Theatrical release.
        /// </summary>
        Theatrical = 3,

        /// <summary>
        /// Digital release.
        /// </summary>
        Digital = 4,

        /// <summary>
        /// Physical release.
        /// </summary>
        Physical = 5,

        /// <summary>
        /// TV release.
        /// </summary>
        TV = 6
    }
}
