using System.ComponentModel;

namespace TMDbLib.Objects.General
{
    public enum MediaType
    {
        Unknown,

        [Description("movie")]
        Movie,

        [Description("tv")]
        TVShow
    }
}