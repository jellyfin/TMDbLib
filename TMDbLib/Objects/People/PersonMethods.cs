using System;
using System.ComponentModel;

namespace TMDbLib.Objects.People
{
    [Flags]
    public enum PersonMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("movie_credits")]
        MovieCredits = 1,
        [Description("tv_credits")]
        TvCredits = 2,
        [Description("external_ids")]
        ExternalIds = 4,
        [Description("images")]
        Images = 8,
        [Description("tagged_images")]
        TaggedImages = 16,
        [Description("changes")]
        Changes = 32
    }
}