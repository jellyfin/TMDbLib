using System.ComponentModel;

namespace TMDbLib.Objects.Movies
{
    public enum TvShowListType
    {
        [Description("on_the_air")]
        OnTheAir,
        [Description("airing_today")]
        AiringToday,
        [Description("top_rated")]
        TopRated,
        [Description("popular")]
        Popular
    }
}