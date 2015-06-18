using System.ComponentModel;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverTvShowSortBy
    {
        [Description("vote_average.asc")]
        VoteAverage,
        [Description("vote_average.desc")]
        VoteAverageDesc,
        [Description("first_air_date.asc")]
        FirstAirDate,
        [Description("first_air_date.desc")]
        FirstAirDateDesc,
        [Description("popularity.asc")]
        Popularity,
        [Description("popularity.desc")]
        PopularityDesc
    }
}
