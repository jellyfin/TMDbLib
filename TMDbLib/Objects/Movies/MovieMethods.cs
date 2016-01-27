using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Movies
{
    [Flags]
    public enum MovieMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "alternative_titles")]
        AlternativeTitles = 1,
        [Display(Description = "credits")]
        Credits = 2,
        [Display(Description = "images")]
        Images = 4,
        [Display(Description = "keywords")]
        Keywords = 8,
        [Display(Description = "releases")]
        Releases = 16,
        [Display(Description = "videos")]
        Videos = 32,
        [Display(Description = "translations")]
        Translations = 64,
        [Display(Description = "similar")]
        Similar = 128,
        [Display(Description = "reviews")]
        Reviews = 256,
        [Display(Description = "lists")]
        Lists = 512,
        [Display(Description = "changes")]
        Changes = 1024,
        /// <summary>
        /// Requires a valid user session to be set on the client object
        /// </summary>
        [Display(Description = "account_states")]
        AccountStates = 2048,
        [Display(Description = "release_dates")]
        ReleaseDates = 4096
    }
}