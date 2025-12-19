using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Specifies the sorting options for movie discovery queries.
/// </summary>
public enum DiscoverMovieSortBy
{
    /// <summary>
    /// No specific sorting defined.
    /// </summary>
    Undefined,

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
    /// Sort by release date in ascending order.
    /// </summary>
    [EnumValue("release_date.asc")]
    ReleaseDate,

    /// <summary>
    /// Sort by release date in descending order.
    /// </summary>
    [EnumValue("release_date.desc")]
    ReleaseDateDesc,

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
    /// Sort by original title in ascending order.
    /// </summary>
    [EnumValue("original_title.asc")]
    OriginalTitle,

    /// <summary>
    /// Sort by original title in descending order.
    /// </summary>
    [EnumValue("original_title.desc")]
    OriginalTitleDesc,

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
