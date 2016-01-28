using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverMovieSortBy
    {
        [Obsolete]
        Undefined,
        [Display(Description = "popularity.asc")]
        Popularity,
        [Display(Description = "popularity.desc")]
        PopularityDesc,
        [Display(Description = "release_date.asc")]
        ReleaseDate,
        [Display(Description = "release_date.desc")]
        ReleaseDateDesc,
        [Display(Description = "revenue.asc")]
        Revenue,
        [Display(Description = "revenue.desc")]
        RevenueDesc,
        [Display(Description = "primary_release_date.asc")]
        PrimaryReleaseDate,
        [Display(Description = "primary_release_date.desc")]
        PrimaryReleaseDateDesc,
        [Display(Description = "original_title.asc")]
        OriginalTitle,
        [Display(Description = "original_title.desc")]
        OriginalTitleDesc,
        [Display(Description = "vote_average.asc")]
        VoteAverage,
        [Display(Description = "vote_average.desc")]
        VoteAverageDesc,
        [Display(Description = "vote_count.asc")]
        VoteCount,
        [Display(Description = "vote_count.desc")]
        VoteCountDesc
    }
}
