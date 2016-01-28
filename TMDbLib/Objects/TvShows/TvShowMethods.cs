using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.TvShows
{
    [Flags]
    public enum TvShowMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "credits")]
        Credits = 1,
        [Display(Description = "images")]
        Images = 2,
        [Display(Description = "external_ids")]
        ExternalIds = 4,
        [Display(Description = "content_ratings")]
        ContentRatings = 8,
        [Display(Description = "alternative_titles")]
        AlternativeTitles = 16,
        [Display(Description = "keywords")]
        Keywords = 32,
        [Display(Description = "similar")]
        Similar = 64,
        [Display(Description = "videos")]
        Videos = 128,
        [Display(Description = "translations")]
        Translations = 256,
        [Display(Description = "account_states")]
        AccountStates = 512,
        [Display(Description = "changes")]
        Changes = 1024
    }
}
