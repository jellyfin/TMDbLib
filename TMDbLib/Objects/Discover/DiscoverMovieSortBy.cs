using System.ComponentModel;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverMovieSortBy
    {
        Undefined = 0,
        [Description("vote_average.desc")]
        VoteAverageDescending = 1,
        [Description("vote_average.asc")]
        VoteAverageAscending = 2,
        [Description("release_date.desc")]
        ReleaseDateDescending = 3,
        [Description("release_date.asc")]
        ReleaseDateAscending = 4,
        [Description("popularity.desc")]
        PopularityDescending = 5,
        [Description("popularity.asc")]
        PopularityAscending = 6
    }
}
