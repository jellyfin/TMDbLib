using System.ComponentModel;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverTvShowSortBy
    {
        Undefined = 0,
        [Description("vote_average.desc")]
        VoteAverageDescending = 1,
        [Description("vote_average.asc")]
        VoteAverageAscending = 2,
        [Description("first_air_date.desc")]
        FirstAirDateDescending = 3,
        [Description("first_air_date.asc")]
        FirstAirDateAscending = 4,
        [Description("popularity.desc")]
        PopularityDescending = 5,
        [Description("popularity.asc")]
        PopularityAscending = 6
    }
}
