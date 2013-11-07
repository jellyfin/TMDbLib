using System;
using System.ComponentModel;

namespace TMDbLib.Objects.Movies
{
    [Flags]
    public enum MovieMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("alternative_titles")]
        AlternativeTitles = 1,
        [Description("credits")]
        Credits = 2,
        [Description("images")]
        Images = 4,
        [Description("keywords")]
        Keywords = 8,
        [Description("releases")]
        Releases = 16,
        [Description("trailers")]
        Trailers = 32,
        [Description("translations")]
        Translations = 64,
        [Description("similar_movies")]
        SimilarMovies = 128,
        [Description("lists")]
        Lists = 256,
        [Description("changes")]
        Changes = 512
    }
}