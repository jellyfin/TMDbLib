using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.TvShows
{
    [Flags]
    public enum TvEpisodeMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "credits")]
        Credits = 1,
        [Display(Description = "images")]
        Images = 2,
        [Display(Description = "external_ids")]
        ExternalIds = 4,
        [Display(Description = "videos")]
        Videos = 8,
        [Display(Description = "account_states")]
        AccountStates = 16,
    }
}
