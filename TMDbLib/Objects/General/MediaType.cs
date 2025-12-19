using Newtonsoft.Json;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the type of media.
/// </summary>
[JsonConverter(typeof(EnumStringValueConverter))]
public enum MediaType
{
    /// <summary>
    /// Unknown media type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Movie media type.
    /// </summary>
    [EnumValue("movie")]
    Movie = 1,

    /// <summary>
    /// TV show media type.
    /// </summary>
    [EnumValue("tv")]
    Tv = 2,

    /// <summary>
    /// Person media type.
    /// </summary>
    [EnumValue("person")]
    Person = 3,

    /// <summary>
    /// Episode media type.
    /// </summary>
    [EnumValue("episode")]
    Episode = 4,

    /// <summary>
    /// TV episode media type.
    /// </summary>
    [EnumValue("tv_episode")]
    TvEpisode = 5,

    /// <summary>
    /// Season media type.
    /// </summary>
    [EnumValue("season")]
    Season = 6,

    /// <summary>
    /// TV season media type.
    /// </summary>
    [EnumValue("tv_season")]
    TvSeason = 7,

    /// <summary>
    /// Collection media type.
    /// </summary>
    [EnumValue("collection")]
    Collection = 8
}
