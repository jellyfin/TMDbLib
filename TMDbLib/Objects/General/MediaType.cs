using TMDbLib.Utilities;

namespace TMDbLib.Objects.General
{
    public enum MediaType
    {
        Unknown,

        [EnumValue("movie")]
        Movie,

        [EnumValue("tv")]
        TVShow
    }
}