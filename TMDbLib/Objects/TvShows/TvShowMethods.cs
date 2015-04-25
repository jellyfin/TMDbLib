using System;
using System.ComponentModel;

namespace TMDbLib.Objects.TvShows
{
    [Flags]
    public enum TvShowMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("credits")]
        Credits = 1,
        [Description("images")]
        Images = 2,
        [Description("external_ids")]
        ExternalIds = 4,
        [Description("content_ratings")]
        ContentRatings = 8,
        [Description("alternative_titles")]
        AlternativeTitles = 16,
        [Description("keywords")]
        Keywords = 32,
        [Description("similar")]
        Similar = 64,
        [Description("videos")]
        Videos = 128,
    }
}
