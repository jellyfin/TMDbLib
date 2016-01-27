using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.People
{
    [Flags]
    public enum PersonMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "movie_credits")]
        MovieCredits = 1,
        [Display(Description = "tv_credits")]
        TvCredits = 2,
        [Display(Description = "external_ids")]
        ExternalIds = 4,
        [Display(Description = "images")]
        Images = 8,
        [Display(Description = "tagged_images")]
        TaggedImages = 16,
        [Display(Description = "changes")]
        Changes = 32
    }
}