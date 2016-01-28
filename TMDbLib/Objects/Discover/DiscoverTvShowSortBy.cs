using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Discover
{
    public enum DiscoverTvShowSortBy
    {
        [Obsolete]
        Undefined,
        [Display(Description = "vote_average.asc")]
        VoteAverage,
        [Display(Description = "vote_average.desc")]
        VoteAverageDesc,
        [Display(Description = "first_air_date.asc")]
        FirstAirDate,
        [Display(Description = "first_air_date.desc")]
        FirstAirDateDesc,
        [Display(Description = "popularity.asc")]
        Popularity,
        [Display(Description = "popularity.desc")]
        PopularityDesc
    }
}
