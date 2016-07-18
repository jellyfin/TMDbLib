using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.TvShows
{
    [Flags]
    public enum TvShowMethods
    {
        [EnumValue("Undefined")]
        Undefined = 0,
        [EnumValue("credits")]
        Credits = 1,
        [EnumValue("images")]
        Images = 2,
        [EnumValue("external_ids")]
        ExternalIds = 4,
        [EnumValue("content_ratings")]
        ContentRatings = 8,
        [EnumValue("alternative_titles")]
        AlternativeTitles = 16,
        [EnumValue("keywords")]
        Keywords = 32,
        [EnumValue("similar")]
        Similar = 64,
        [EnumValue("videos")]
        Videos = 128,
        [EnumValue("translations")]
        Translations = 256,
        [EnumValue("account_states")]
        AccountStates = 512,
        [EnumValue("changes")]
        Changes = 1024
    }
}
