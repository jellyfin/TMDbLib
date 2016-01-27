using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.General
{
    public enum MediaType
    {
        Unknown,

        [Display(Description = "movie")]
        Movie,

        [Display(Description = "tv")]
        TVShow
    }
}