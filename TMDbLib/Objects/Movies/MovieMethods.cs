using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Movies
{
    [Flags]
    public enum MovieMethods
    {
        [EnumValue("Undefined")]
        Undefined = 0,
        [EnumValue("alternative_titles")]
        AlternativeTitles = 1,
        [EnumValue("credits")]
        Credits = 2,
        [EnumValue("images")]
        Images = 4,
        [EnumValue("keywords")]
        Keywords = 8,
        [EnumValue("releases")]
        Releases = 16,
        [EnumValue("videos")]
        Videos = 32,
        [EnumValue("translations")]
        Translations = 64,
        [EnumValue("similar")]
        Similar = 128,
        [EnumValue("reviews")]
        Reviews = 256,
        [EnumValue("lists")]
        Lists = 512,
        [EnumValue("changes")]
        Changes = 1024,
        /// <summary>
        /// Requires a valid user session to be set on the client object
        /// </summary>
        [EnumValue("account_states")]
        AccountStates = 2048,
        [EnumValue("release_dates")]
        ReleaseDates = 4096
    }
}