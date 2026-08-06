using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// TV show list type.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<TvShowListType>))]
public enum TvShowListType
{
    /// <summary>
    /// TV shows currently on the air.
    /// </summary>
    [EnumValue("on_the_air")]
    OnTheAir,

    /// <summary>
    /// TV shows airing today.
    /// </summary>
    [EnumValue("airing_today")]
    AiringToday,

    /// <summary>
    /// Top rated TV shows.
    /// </summary>
    [EnumValue("top_rated")]
    TopRated,

    /// <summary>
    /// Popular TV shows.
    /// </summary>
    [EnumValue("popular")]
    Popular
}
