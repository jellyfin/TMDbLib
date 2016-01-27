using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Movies
{
    public enum TvShowListType
    {
        [Display(Description = "on_the_air")]
        OnTheAir,
        [Display(Description = "airing_today")]
        AiringToday,
        [Display(Description = "top_rated")]
        TopRated,
        [Display(Description = "popular")]
        Popular
    }
}