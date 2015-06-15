using System.ComponentModel;

namespace TMDbLib.Objects.General
{
    public enum MediaType
    {
        [Description("movie")]
        Movie,
        [Description("tv")]
        TVShow,
        Unknown
    }
}