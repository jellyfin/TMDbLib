using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

public enum DiscoverTvShowSortBy
{
    Undefined,
    [EnumValue("vote_average.asc")]
    VoteAverage,
    [EnumValue("vote_average.desc")]
    VoteAverageDesc,
    [EnumValue("first_air_date.asc")]
    FirstAirDate,
    [EnumValue("first_air_date.desc")]
    FirstAirDateDesc,
    [EnumValue("popularity.asc")]
    Popularity,
    [EnumValue("popularity.desc")]
    PopularityDesc,
    [EnumValue("revenue.asc")]
    Revenue,
    [EnumValue("revenue.desc")]
    RevenueDesc,
    [EnumValue("primary_release_date.asc")]
    PrimaryReleaseDate,
    [EnumValue("primary_release_date.desc")]
    PrimaryReleaseDateDesc,
    [EnumValue("vote_count.asc")]
    VoteCount,
    [EnumValue("vote_count.desc")]
    VoteCountDesc
}
