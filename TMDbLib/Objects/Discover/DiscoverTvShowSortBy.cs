using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Specifies the sorting options for TV show discovery queries.
/// </summary>
public enum DiscoverTvShowSortBy
{
    /// <summary>
    /// No specific sorting defined.
    /// </summary>
    Undefined,

    /// <summary>
    /// Sort by vote average in ascending order.
    /// </summary>
    [EnumValue("vote_average.asc")]
    VoteAverage,

    /// <summary>
    /// Sort by vote average in descending order.
    /// </summary>
    [EnumValue("vote_average.desc")]
    VoteAverageDesc,

    /// <summary>
    /// Sort by first air date in ascending order.
    /// </summary>
    [EnumValue("first_air_date.asc")]
    FirstAirDate,

    /// <summary>
    /// Sort by first air date in descending order.
    /// </summary>
    [EnumValue("first_air_date.desc")]
    FirstAirDateDesc,

    /// <summary>
    /// Sort by popularity in ascending order.
    /// </summary>
    [EnumValue("popularity.asc")]
    Popularity,

    /// <summary>
    /// Sort by popularity in descending order.
    /// </summary>
    [EnumValue("popularity.desc")]
    PopularityDesc,

    /// <summary>
    /// Sort by revenue in ascending order.
    /// </summary>
    [EnumValue("revenue.asc")]
    Revenue,

    /// <summary>
    /// Sort by revenue in descending order.
    /// </summary>
    [EnumValue("revenue.desc")]
    RevenueDesc,

    /// <summary>
    /// Sort by primary release date in ascending order.
    /// </summary>
    [EnumValue("primary_release_date.asc")]
    PrimaryReleaseDate,

    /// <summary>
    /// Sort by primary release date in descending order.
    /// </summary>
    [EnumValue("primary_release_date.desc")]
    PrimaryReleaseDateDesc,

    /// <summary>
    /// Sort by vote count in ascending order.
    /// </summary>
    [EnumValue("vote_count.asc")]
    VoteCount,

    /// <summary>
    /// Sort by vote count in descending order.
    /// </summary>
    [EnumValue("vote_count.desc")]
    VoteCountDesc
}
