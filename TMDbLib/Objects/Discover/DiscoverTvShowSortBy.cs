using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Specifies the sorting options for TV show discovery queries.
/// </summary>
[TolerantEnum]
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
    /// Sort by name in ascending order.
    /// </summary>
    [EnumValue("name.asc")]
    Name,

    /// <summary>
    /// Sort by name in descending order.
    /// </summary>
    [EnumValue("name.desc")]
    NameDesc,

    /// <summary>
    /// Sort by original name in ascending order.
    /// </summary>
    [EnumValue("original_name.asc")]
    OriginalName,

    /// <summary>
    /// Sort by original name in descending order.
    /// </summary>
    [EnumValue("original_name.desc")]
    OriginalNameDesc,

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
