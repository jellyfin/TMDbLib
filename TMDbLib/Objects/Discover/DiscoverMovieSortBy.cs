using System.ComponentModel;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverMovieSortBy
    {
        [Description("popularity.asc")]
        Popularity,
        [Description("popularity.desc")]
        PopularityDesc,
        [Description("release_date.asc")]
        ReleaseDate,
        [Description("release_date.desc")]
        ReleaseDateDesc,
        [Description("revenue.asc")]
        Revenue,
        [Description("revenue.desc")]
        RevenueDesc,
        [Description("primary_release_date.asc")]
        PrimaryReleaseDate,
        [Description("primary_release_date.desc")]
        PrimaryReleaseDateDesc,
        [Description("original_title.asc")]
        OriginalTitle,
        [Description("original_title.desc")]
        OriginalTitleDesc,
        [Description("vote_average.asc")]
        VoteAverage,
        [Description("vote_average.desc")]
        VoteAverageDesc,
        [Description("vote_count.asc")]
        VoteCount,
        [Description("vote_count.desc")]
        VoteCountDesc
    }
}
